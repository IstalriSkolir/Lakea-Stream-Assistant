import math

def create_bounding_box(point_1: tuple, point_2: tuple) -> list:
    if point_1[0] < point_2[0] and point_1[1] < point_2[1]:
        return [point_1[0], point_1[1], point_2[0], point_2[1]]
    elif point_1[0] >= point_2[0] and point_1[1] < point_2[1]:
        return [point_2[0], point_1[1], point_1[0], point_2[1]]
    elif point_1[0] < point_2[0] and point_1[1] >= point_2[1]:
        return [point_1[0], point_2[1], point_2[0], point_1[1]]
    else:
        return [point_2[0], point_2[1], point_1[0], point_1[1]]

def create_square_bounding_box(point_1: tuple, point_2: tuple) -> list:
    bounding_box = create_bounding_box(point_1, point_2)
    x_difference = bounding_box[2] - bounding_box[0]
    y_difference = bounding_box[3] - bounding_box[1]
    if x_difference == y_difference:
        return bounding_box
    elif x_difference > y_difference:
        offset = (x_difference - y_difference) / 2
        return [bounding_box[0], bounding_box[1] - offset, bounding_box[2], bounding_box[3] + offset]
    else:
        offset = (y_difference - x_difference) /2
        return [bounding_box[0] - offset, bounding_box[1], bounding_box[2] + offset, bounding_box[3]]

def widen_bounding_box(bounding_box: list, width: float) -> list:
    half_width = width / 2
    return [bounding_box[0] - half_width, bounding_box[1] - half_width, bounding_box[2] + half_width, + bounding_box[3] + half_width]

def calculate_distance_between_points(point_1: tuple, point_2: tuple) -> float:
    x_difference = point_1[0] - point_2[0]
    y_difference = point_1[1] - point_2[1]
    return math.sqrt(y_difference * y_difference + x_difference * x_difference) 

def move_point_towards_point(point_1: tuple, point_2: tuple , amount: float) -> tuple:
    x_difference = point_2[0] - point_1[0]
    y_difference = point_2[1] - point_1[1]
    x_position = point_1[0] + (x_difference * amount)
    y_position = point_1[1] + (y_difference * amount)    
    return (x_position, y_position)

def is_point_inside_rectangle(point_1, point_2, point_3, point_4, check_point):
    d1 = is_point_on_left(point_1, point_2, check_point)
    d2 = is_point_on_left(point_2, point_3, check_point)
    d3 = is_point_on_left(point_3, point_4, check_point)
    d4 = is_point_on_left(point_4, point_1, check_point)
    if d1 > 0 or d2 > 0 or d3 > 0 or d4 > 0:
        return False
    else:
        return True
    
def is_point_on_left(point_1: tuple, point_2: tuple, check_point: tuple) -> float:
    return (point_2[0] - point_1[0]) * (check_point[1] - point_1[1]) - (check_point[0] - point_1[0]) * (point_2[1] - point_1[1])

def calculate_line_gradient_from_points(point_1: tuple, point_2: tuple) -> float:
    x_difference = point_1[0] - point_2[0]
    y_difference = point_1[1] - point_2[1]
    return y_difference / x_difference

def calculate_y_intercept(point_1: tuple, gradient: float) -> float:
    value = point_1[0] * gradient
    return point_1[1] - value

# https://stackoverflow.com/questions/20677795/how-do-i-compute-the-intersection-point-of-two-lines
def calculate_intersection_between_lines(line_1_start: tuple, line_1_end: tuple, line_2_start: tuple, line_2_end: tuple) -> tuple:
    line_1_gradient = calculate_line_gradient_from_points(line_1_start, line_1_end)
    line_2_gradient = calculate_line_gradient_from_points(line_2_start, line_2_end)
    line_1_y_intercept = calculate_y_intercept(line_1_start, line_1_gradient)
    line_2_y_intercept = calculate_y_intercept(line_2_start, line_2_gradient)
