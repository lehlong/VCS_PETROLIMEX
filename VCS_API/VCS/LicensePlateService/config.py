######### LICENSE PLATE RECOGNITION #######

config_paddle_ocr_rec = {
    "model_path": "./checkpoints/paddle_ocr_reg_onnx/model.onnx",
    "img_height": 48,
    "img_width": 120,
    "vocabulary": ['blank', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ':', ';', '<', '=', '>', '?',
                          '@', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O',
                          'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '[', '\\', ']', '^', '_',
                          '`', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
                          'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '{', '|', '}', '~', '!', '"', '#',
                          '$', '%', '&', "'", '(', ')', '*', '+', ',', '-', '.', '/', ' ', ' '],
    "batch_size": 32,
    "rgb": False,
    "gpu": 1,
}

config_lp_ocr_det = {
    "model_path": "./checkpoints/paddle_ocr_det_onnx/model.onnx",
    "binary_threshold": 0.7,
    "polygon_threshold": 0.7,
    "un_clip_ratio": 0.1,
    "min_size": 128,
    "max_candidates": 5000,
    "result_type": "rectangle",
    "db_plus_plus": True,
    "gpu": 1,
}


config_lp_det =  {
    "model_path": "./checkpoints/license_plate_det_onnx/model.onnx",
    "class_name": [
        'lp'
    ],
    "input_shape": (640, 640),
    "iou_threshold": 0.7,
    "conf_threshold": 0.6
}