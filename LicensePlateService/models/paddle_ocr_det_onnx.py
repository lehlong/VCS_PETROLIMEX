import numpy as np
import cv2
import time
import pyclipper
import onnxruntime
from shapely.geometry import Polygon
from functools import partial


class PaddleDetOnnx:
    def __init__(
                self,
                model_path: str,
                min_size,
                binary_threshold: float=0.5,
                polygon_threshold: float = 0.5,
                gpu=-1,
                **kwargs
                ):
        if gpu > -1:
            self.ort_session = onnxruntime.InferenceSession(
                model_path, providers=["CPUExecutionProvider"]
            )
            # self.ort_session.set_providers(['CUDAExecutionProvider'])
        else:
            # self.ort_session.set_providers(['CPUExecutionProvider'])
            self.ort_session = onnxruntime.InferenceSession(
                model_path, providers=["CPUExecutionProvider"]
            )
        self.min_size_box = 3
        self.min_size = min_size 
        self.max_size = 960
        self.box_thresh = polygon_threshold
        self.mask_thresh = binary_threshold

        self.mean = np.array([123.675, 116.28, 103.53])  # imagenet mean
        self.mean = self.mean.reshape(1, -1).astype('float64')

        self.std = np.array([58.395, 57.12, 57.375])  # imagenet std
        self.std = 1 / self.std.reshape(1, -1).astype('float64')
        self.timeout = 10

    def filter_polygon(self, points, shape):
        width = shape[1]
        height = shape[0]
        filtered_points = []
        for point in points:
            if type(point) is list:
                point = np.array(point)
            point = self.clockwise_order(point)
            point = self.clip(point, height, width)
            w = np.linalg.norm(point[0] - point[1])
            h = np.linalg.norm(point[0] - point[3])
            if w <= 0.001 or h <= 0.001:
                continue
            filtered_points.append(point)
        return np.array(filtered_points)

    def boxes_from_bitmap(self, output, mask, dest_width, dest_height):
        mask = (mask * 255).astype(np.uint8)
        height, width = mask.shape
        outs = cv2.findContours(mask, cv2.RETR_LIST, cv2.CHAIN_APPROX_SIMPLE)
        if len(outs) == 2:
            contours = outs[0]
        else:
            contours = outs[1]

        boxes = []
        scores = []
        print("contours", len(contours))
        for index in range(len(contours)):
            contour = contours[index]
            points, min_side = self.get_min_boxes(contour)
            if min_side < self.min_size_box:
                continue
            points = np.array(points)
            score = self.box_score(output, contour)
            if self.box_thresh > score:
                continue

            polygon = Polygon(points)
            distance = polygon.area / polygon.length
            offset = pyclipper.PyclipperOffset()
            offset.AddPath(points, pyclipper.JT_ROUND, pyclipper.ET_CLOSEDPOLYGON)
            points = np.array(offset.Execute(distance * 1.5)).reshape((-1, 1, 2))

            box, min_side = self.get_min_boxes(points)
            print("boxes_from_bitmap minside", min_side)
            if min_side < self.min_size_box + 2:
                continue
            box = np.array(box)
            box[:, 0] = np.clip(box[:, 0] / width * dest_width, 0, dest_width)
            box[:, 1] = np.clip(box[:, 1] / height * dest_height, 0, dest_height)
            boxes.append(box.astype(np.float32))
            scores.append(score)
        return np.array(boxes, dtype=np.float32), scores

    @staticmethod
    def get_min_boxes(contour):
        bounding_box = cv2.minAreaRect(contour)
        points = sorted(list(cv2.boxPoints(bounding_box)), key=lambda x: x[0])

        if points[1][1] > points[0][1]:
            index_1 = 0
            index_4 = 1
        else:
            index_1 = 1
            index_4 = 0
        if points[3][1] > points[2][1]:
            index_2 = 2
            index_3 = 3
        else:
            index_2 = 3
            index_3 = 2

        box = [points[index_1], points[index_2], points[index_3], points[index_4]]
        return box, min(bounding_box[1])

    @staticmethod
    def box_score(bitmap, contour):
        h, w = bitmap.shape[:2]
        contour = contour.copy()
        contour = np.reshape(contour, (-1, 2))

        x1 = np.clip(np.min(contour[:, 0]), 0, w - 1)
        y1 = np.clip(np.min(contour[:, 1]), 0, h - 1)
        x2 = np.clip(np.max(contour[:, 0]), 0, w - 1)
        y2 = np.clip(np.max(contour[:, 1]), 0, h - 1)

        mask = np.zeros((y2 - y1 + 1, x2 - x1 + 1), dtype=np.uint8)

        contour[:, 0] = contour[:, 0] - x1
        contour[:, 1] = contour[:, 1] - y1
        contour = contour.reshape((1, -1, 2)).astype("int32")

        cv2.fillPoly(mask, contour, color=(1, 1))
        return cv2.mean(bitmap[y1:y2 + 1, x1:x2 + 1], mask)[0]

    @staticmethod
    def clockwise_order(point):
        poly = np.zeros((4, 2), dtype="float32")
        s = point.sum(axis=1)
        poly[0] = point[np.argmin(s)]
        poly[2] = point[np.argmax(s)]
        tmp = np.delete(point, (np.argmin(s), np.argmax(s)), axis=0)
        diff = np.diff(np.array(tmp), axis=1)
        poly[1] = tmp[np.argmin(diff)]
        poly[3] = tmp[np.argmax(diff)]
        return poly

    @staticmethod
    def clip(points, h, w):
        for i in range(points.shape[0]):
            points[i, 0] = min(max(points[i, 0], 0), w)
            points[i, 1] = min(max(points[i, 1], 0), h)
        return points

    def resize(self, image):
        h, w = image.shape[:2]

        # limit the max side
        ratio = 1.
        if max(h, w) > self.max_size:
            if h > w:
                ratio = float(self.max_size) / h
            else:
                ratio = float(self.max_size) / w
        elif max(h, w) < self.min_size:
            if h > w:
                ratio = float(self.min_size) / h
            else:
                ratio = float(self.min_size) / w

        resize_h = max(int(round(int(h * ratio) / 32) * 32), 32)
        resize_w = max(int(round(int(w * ratio) / 32) * 32), 32)

        return cv2.resize(image, (resize_w, resize_h))

    @staticmethod
    def zero_pad(image):
        h, w, c = image.shape
        pad = np.zeros((max(32, h), max(32, w), c), np.uint8)
        pad[:h, :w, :] = image
        return pad
    
    def run(self, images, min_size=148, polygon_threshold=0.5, binary_threshold=0.5):
        self.min_size = min_size 
        self.box_thresh = polygon_threshold
        self.mask_thresh = binary_threshold
        bboxes = []
        bbox_confs = []
        result_count = []
        for i in range(len(images)):
            bboxes.append([])
            bbox_confs.append([])
        for i, x in enumerate(images):
            h, w = x.shape[:2]

            if sum([h, w]) < 64:
                x = self.zero_pad(x)

            x = self.resize(x)
            x = x.astype('float32')

            cv2.subtract(x, self.mean, x)  # inplace
            cv2.multiply(x, self.std, x)  # inplace

            
            input_data = np.expand_dims(np.transpose(x, (2, 0, 1)), axis=0)
            ort_inputs = {self.ort_session.get_inputs()[0].name: input_data}
            output = self.ort_session.run(None, ort_inputs)
            predict = output[0]
            outputs = predict.squeeze()
            boxes, scores = self.boxes_from_bitmap(outputs, outputs > self.mask_thresh, 1, 1)
            boxes = self.filter_polygon(boxes, (1, 1))
            bboxes[i] = boxes
            bbox_confs[i] = scores
        return bboxes, bbox_confs