import math
from PIL import ImageDraw
from Utils.maths import is_point_inside_rectangle, create_square_bounding_box, widen_bounding_box, move_point_towards_point, calculate_line_gradient_from_points, calculate_y_intercept, calculate_intersection_between_lines

def draw_rope(image, start, end, thickness):
    draw = ImageDraw.Draw(image)

    line_width = int(thickness * 0.15)
    colour_width = int(thickness) #- 1
    line_angle_radians = math.atan2(end[1] - start[1], end[0] - start[0])
    line_angle_degrees = math.degrees(line_angle_radians)
    radius = thickness / 2

    x_offset = radius * math.cos(line_angle_radians + 1.5708)
    y_offset = radius * math.sin(line_angle_radians + 1.5708)

    line_1_start = (start[0] + x_offset, start[1] + y_offset)
    line_1_end = (end[0] + x_offset, end[1] + y_offset)
    line_2_start = (start[0] - x_offset, start[1] - y_offset)
    line_2_end = (end[0] - x_offset, end[1] - y_offset)

    #radius_reduced =  radius - (line_width / 2)
    #x_offset_reduced = radius_reduced * math.cos(line_angle_radians + 1.5708)
    #y_offset_reduced = radius_reduced * math.sin(line_angle_radians + 1.5708)

    #body_side_1_start = (start[0] + x_offset_reduced, start[1] + y_offset_reduced)
    #body_side_1_end = (end[0] + x_offset_reduced, end[1] + y_offset_reduced)
    #body_side_2_start = (start[0] - x_offset_reduced, start[1] - y_offset_reduced)
    #body_side_2_end = (end[0] - x_offset_reduced, end[1] - y_offset_reduced)

    #draw_body(draw, start, end, body_side_1_start, body_side_1_end, body_side_2_start, body_side_2_end, colour_width, line_width)
    draw_body(draw, start, end, line_1_start, line_1_end, line_2_start, line_2_end, colour_width, line_width)
    draw_ends(draw, line_1_start, line_1_end, line_2_start, line_2_end, line_width, thickness, line_angle_degrees, line_angle_radians)
    draw_details(draw, start, end, line_1_start, line_1_end, line_2_start, line_2_end, line_angle_radians, thickness)

    return image
    
def draw_body(draw, start, end, line_1_start, line_1_end, line_2_start, line_2_end, colour_width, line_width):        
    draw.line((start, end), fill=(187, 161, 121), width=colour_width)
    draw.line((line_1_start, line_1_end), fill=(0, 0, 0), width=line_width)
    draw.line((line_2_start, line_2_end), fill=(0, 0, 0), width=line_width)

def draw_ends(draw, line_1_start, line_1_end, line_2_start, line_2_end, line_width, thickness, line_angle_degrees, line_angle_radians):
    start_bounding_box_inner = create_square_bounding_box(line_1_start, line_2_start)
    start_bounding_box_outer = widen_bounding_box(start_bounding_box_inner, (line_width * 2) * math.sin(line_angle_radians + 0.7854))

    end_bounding_box_inner = create_square_bounding_box(line_1_end, line_2_end)
    end_bounding_box_outer = widen_bounding_box(end_bounding_box_inner, (line_width * 2) * math.sin(line_angle_radians + 0.7854))

    draw.chord(start_bounding_box_outer, 75 + line_angle_degrees, 285 + line_angle_degrees, fill=(0, 0, 0))
    draw.chord(start_bounding_box_inner, 60 + line_angle_degrees, 300 + line_angle_degrees, fill=(187, 161, 121))
    
    draw.chord(end_bounding_box_outer, 255 + line_angle_degrees, 465 + line_angle_degrees, fill=(0, 0, 0))
    draw.chord(end_bounding_box_inner, 240 + line_angle_degrees, 480 + line_angle_degrees, fill=(187, 161, 121))

def draw_details(draw, start, end, line_1_start, line_1_end, line_2_start, line_2_end, line_angle_radians, thickness):
    detail_width = int(thickness * 0.08)
    rope_length = math.dist(start, end)
    steps = int(math.trunc(rope_length / (1.75 * thickness)))
    if steps > 0:
        step = rope_length / steps
        half_step = step / 2
        for x in range(steps + 1):
            new_point_1 = (line_1_start[0] + (((step * x) + half_step) * math.cos(line_angle_radians)), line_1_start[1] + (((step * x) + half_step) * math.sin(line_angle_radians)))
            new_point_2 = (line_2_start[0] + (((step * x) - half_step) * math.cos(line_angle_radians)), line_2_start[1] + (((step * x) - half_step) * math.sin(line_angle_radians)))
            if x == 0:
                new_point_2 = move_point_towards_point(new_point_2, new_point_1, 0.25)
                draw.line((new_point_1, new_point_2), fill=(0, 0, 0), width=detail_width)
            elif x == steps:
                new_point_1 = move_point_towards_point(new_point_1, new_point_2, 0.25)
                draw.line((new_point_1, new_point_2), fill=(0, 0, 0), width=detail_width)
            else:
                draw.line((new_point_1, new_point_2), fill=(0, 0, 0), width=detail_width)

            
            
            #d1 = is_point_inside_rectangle(line_1_start, line_1_end, line_2_end, line_2_start, new_point_1)
            #d2 = is_point_inside_rectangle(line_1_start, line_1_end, line_2_end, line_2_start, new_point_2)
         
            #if d1 == True and d2 == True:
            #    draw.line((new_point_1, new_point_2), fill=(0, 0, 0), width=detail_width)
            #elif d1 == True and d2 == False:
            #    pass

                #point_2_moved = move_point_towards_point(new_point_2, new_point_1, 0.5)

                #draw.line((new_point_1, point_2_moved), fill=(0, 0, 0), width=detail_width)

                #intersection = calculate_intersection_between_lines(new_point_1, new_point_2, )

                #gradient = calculate_line_gradient_from_points(new_point_1, new_point_2)
                #y_intercept = calculate_y_intercept(new_point_1, gradient)
                #print(f"\nFormula -> y = {gradient} * x + {y_intercept}")


            #elif d1 == False and d2 == True:
            #    pass
                
                #draw.line((new_point_1, new_point_2), fill=(0, 0, 0), width=detail_width)

                #gradient = calculate_line_gradient_from_points(new_point_1, new_point_2)
                #y_intercept = calculate_y_intercept(new_point_1, gradient)
                #print(f"\nFormula -> y = {gradient} * x + {y_intercept}\n")

            #else:
            #    pass
                
                #draw.line((new_point_1, new_point_2), fill=(0, 0, 0), width=detail_width)
