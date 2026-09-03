import os
from PIL import Image, ImageDraw

def generate_icon(output_path):
    size = 256
    img = Image.new('RGBA', (size, size), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)

    # Background rounded rectangle with gradient feel
    # Draw outer glow
    draw.rounded_rectangle([8, 8, size - 8, size - 8], radius=50, fill=(15, 23, 42, 255), outline=(56, 189, 248, 220), width=6)
    draw.rounded_rectangle([16, 16, size - 16, size - 16], radius=42, fill=(23, 31, 44, 255), outline=(99, 102, 241, 140), width=4)

    # Draw a stylized globe / network shield
    center_x, center_y = size // 2, size // 2
    
    # Outer orbit ring
    draw.ellipse([45, 45, size - 45, size - 45], outline=(30, 58, 138, 200), width=6)
    draw.ellipse([70, 45, size - 70, size - 45], outline=(56, 189, 248, 160), width=3)
    draw.line([45, center_y, size - 45, center_y], fill=(56, 189, 248, 160), width=3)

    # Central Lightning Bolt (DNS Speed & Power)
    # Vibrant neon amber / electric cyan gradient
    bolt_points = [
        (center_x + 10, 40),
        (center_x - 38, 130),
        (center_x - 5, 130),
        (center_x - 20, 215),
        (center_x + 42, 115),
        (center_x + 8, 115),
    ]
    # Glow shadow
    shadow_points = [(x + 2, y + 3) for x, y in bolt_points]
    draw.polygon(shadow_points, fill=(16, 185, 129, 100))
    
    # Primary bolt fill
    draw.polygon(bolt_points, fill=(251, 191, 36, 255), outline=(245, 158, 11, 255))
    
    # Inner highlight on bolt
    inner_bolt = [
        (center_x + 8, 55),
        (center_x - 28, 126),
        (center_x - 3, 126),
        (center_x - 14, 185),
        (center_x + 30, 118),
        (center_x + 8, 118),
    ]
    draw.polygon(inner_bolt, fill=(254, 240, 138, 220))

    # Dot badges (servers)
    draw.ellipse([36, 75, 48, 87], fill=(16, 185, 129, 255)) # Green node
    draw.ellipse([size - 50, 75, size - 38, 87], fill=(168, 85, 247, 255)) # Purple node
    draw.ellipse([center_x - 6, size - 38, center_x + 6, size - 26], fill=(56, 189, 248, 255)) # Cyan node

    sizes = [(256, 256), (128, 128), (64, 64), (48, 48), (32, 32), (16, 16)]
    img.save(output_path, format='ICO', sizes=sizes)
    print(f"Generated {output_path} successfully.")

if __name__ == '__main__':
    generate_icon(r"c:\Users\hadib\Desktop\Arezoo\src\app.ico")
