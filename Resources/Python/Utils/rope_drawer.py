from PIL import ImageDraw
import math

def draw_rope(image, start, end, thickness):
    draw = ImageDraw.Draw(image)

    line_width = int(thickness * 0.15)
    colour_width = int(thickness) - 1
    line_angle_radians = math.atan2(end[1] - start[1], end[0] - start[0])
    #line_angle_degrees = math.degrees(line_angle_radians)
    radius = thickness / 2

    x_offset = radius * math.cos(line_angle_radians + 1.5708)
    y_offset = radius * math.sin(line_angle_radians + 1.5708)

    line_1_start = (start[0] + x_offset, start[1] + y_offset)
    line_1_end = (end[0] + x_offset, end[1] + y_offset)
    line_2_start = (start[0] - x_offset, start[1] - y_offset)
    line_2_end = (end[0] - x_offset, end[1] - y_offset)

    draw_body(draw, start, end, line_1_start, line_1_end, line_2_start, line_2_end, colour_width, line_width)
    draw_ends(draw, line_1_start, line_1_end, line_2_start, line_2_end, line_width)
    draw_details(draw, start, end, line_1_start, line_2_start, line_angle_radians, thickness)

    return image
    
def draw_body(draw, start, end, line_1_start, line_1_end, line_2_start, line_2_end, colour_width, line_width):    
    draw.line((start, end), fill=(187, 161, 121), width=colour_width)
    draw.line((line_1_start, line_1_end), fill=(0, 0, 0), width=line_width)
    draw.line((line_2_start, line_2_end), fill=(0, 0, 0), width=line_width)

def draw_ends(draw, line_1_start, line_1_end, line_2_start, line_2_end, line_width):
    draw.line((line_1_start, line_2_start), fill=(0, 0, 0), width=line_width)
    draw.line((line_1_end, line_2_end), fill=(0, 0, 0), width=line_width)

def draw_details(draw, start, end, line_1_start, line_2_start, line_angle_radians, thickness):
    detail_width = int(thickness * 0.075)
    rope_length = math.dist(start, end)
    steps = int(math.trunc(rope_length / 30))
    if steps > 0:
        step = rope_length / steps
        half_step = step / 2
        #print(f"\nthickness: {thickness}\ndetail_width: {detail_width}\nrope_length: {rope_length}\nsteps: {steps}\nstep: {step}")
        for x in range(steps):
            new_point_1 = (line_1_start[0] + ((step * x) * math.cos(line_angle_radians)), line_1_start[1] + ((step * x) * math.sin(line_angle_radians)))
            new_point_2 = (line_2_start[0] + ((step * x) * math.cos(line_angle_radians)), line_2_start[1] + ((step * x) * math.sin(line_angle_radians)))
            draw.line((new_point_1, new_point_2), fill=(0, 0, 0), width=detail_width)
            #draw.point(new_point_1, fill=(255, 255, 255))
            #draw.point(new_point_2, fill=(255, 255, 255))
        #print("\n")