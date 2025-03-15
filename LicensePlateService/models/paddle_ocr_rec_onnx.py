import cv2
import numpy as np
import math
import onnxruntime
from functools import partial
from box_post_processing import crop_image
from definitions import TextOCR


class PaddleCrnnOnnx:
    def __init__(
        self, 
        model_path,
        vocabulary,
        img_height,
        img_width,
        batch_size,
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
        self.character = list(vocabulary)
        self.height = img_height
        self.width = img_width
        self.crnn_timeout = 5
        self.batch_size = batch_size
        self.mean = np.array([127.5], dtype=np.float32)

    def preprocess(self, image, max_wh_ratio=1):
        input_h, input_w = self.height, self.width

        input_w = int((input_h * max_wh_ratio))
        input_w = 320
        h, w = image.shape[:2]
        ratio = w / float(h)
        if math.ceil(input_h * ratio) > input_w:
            resized_w = input_w
        else:
            resized_w = int(math.ceil(input_h * ratio))

        resized_image = cv2.resize(image, (resized_w, input_h))
        resized_image = resized_image.transpose((2, 0, 1))
        resized_image = resized_image.astype('float32')
        resized_image = resized_image / 255.0
        resized_image -= 0.5
        resized_image /= 0.5
        padded_image = np.zeros((3, input_h, input_w), dtype=np.float32)
        padded_image[:, :, 0:resized_w] = resized_image
        return padded_image

    def run(self, image, bboxes, img_width):
        text_boxes = []
        self.width = img_width
        image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
        image_h, image_w = image.shape[:2]
        for i in range(0, len(bboxes), self.batch_size):
            inputs_ = []
            texts_ = []
            for j in range(i, min(i+self.batch_size, len(bboxes))):
                bbox = bboxes[j]
                xmin = int(np.min(bbox[:, 0]) * image_w)
                xmax = int(np.max(bbox[:, 0]) * image_w)
                ymin = int(np.min(bbox[:, 1]) * image_h)
                ymax = int(np.max(bbox[:, 1]) * image_h)
                bbox[:, 0] = bbox[:,0]*image_w
                bbox[:, 1] = bbox[:,1]*image_h
                crop_image_ = crop_image(image, bbox)
                if xmax - xmin <= 0 or ymax - ymin <= 0:
                    continue
                text_box = TextOCR(xmin, ymin, xmax, ymax, "", 0)
                texts_.append(text_box)
                input_data = self.preprocess(crop_image_)
                inputs_.append(input_data)
            if len(inputs_) == 0:
                continue
            ort_inputs = {self.ort_session.get_inputs()[0].name: inputs_}
            outputs = self.ort_session.run(None, ort_inputs)
            texts, confidences = self.postprocess(outputs[0])
            for i, ts in enumerate(texts):
                texts_[i].text = texts[i]
                texts_[i].conf_reg = confidences[i]
            text_boxes += texts_
        return text_boxes

    def postprocess(self, inputs):
        batch = inputs.shape[0]
        list_text = []
        list_confidences = []
        for i in range(batch):
            text_index = inputs[i, :, :]
            confidences = self.softmax(text_index, axis=1)
            text_index = np.argmax(confidences, axis=1)
            text, conf = self.decode(text_index, confidences)
            list_text.append(text)
            list_confidences.append(conf)
        return list_text, list_confidences

    def decode(self, text_index, confidences):
        texts = []
        list_char = []
        list_confidences = []
        for i in range(len(text_index)):
            # removing repeated characters and blank.
            if text_index[i] != 0 and (not (i > 0 and text_index[i - 1] == text_index[i])):
                list_char.append(self.character[text_index[i]])
                list_confidences.append(confidences[i, text_index[i]])
        text = ''.join(list_char)
        return text, list_confidences

    def softmax(self, X, theta=1.0, axis=None):
        # make X at least 2d
        y = np.atleast_2d(X)
        # find axis
        if axis is None:
            axis = next(j[0] for j in enumerate(y.shape) if j[1] > 1)
        # multiply y against the theta parameter,
        y = y * float(theta)
        # subtract the max for numerical stability
        y = y - np.expand_dims(np.max(y, axis=axis), axis)
        # exponentiate y
        y = np.exp(y)
        # take the sum along the specified axis
        ax_sum = np.expand_dims(np.sum(y, axis=axis), axis)
        # finally: divide elementwise
        p = y / ax_sum
        # flatten if X was 1D
        if len(X.shape) == 1:
            p = p.flatten()
        return p