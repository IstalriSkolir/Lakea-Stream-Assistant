using Battle_Similator.Models.Creatures;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace Battle_Similator.Models.Resources
{
    public class HealthBarImage
    {
        private IO io;
        private Font font = new Font("Arial Rounded MT Bold", 150);
        private string path;
        private int imageHeight = 500;
        private int imageWidth = 3000;
        private int profileXPos = 0;
        private int profileYPos = 0;
        private int profileImageWidth = 500;
        private int profileImageHeight = 500;
        private int borderXPos = 500;
        private int borderYPos = 0;
        private int textXPos = 500;
        private int textYPos = 250;
        private int borderThickness = 25;
        private int buffer = 5;
        private int barHeight = 225;
        private int barWidth;
        private int barXPos;
        private int barYPos;

        public HealthBarImage(IO io, string config, string resourcePath)
        {
            this.io = io;
            barWidth = imageWidth - (profileImageWidth + borderThickness - buffer);
            barXPos = profileImageWidth + borderThickness - buffer;
            barYPos = imageHeight - (barHeight + borderThickness - buffer) - 25;
            if (config == "LAKEA")
            {
                path = resourcePath;
            }
            else if (config == "DEBUG")
            {
                path = Environment.CurrentDirectory;
            }
        }

        public void GenerateHealthBarImage(Monster boss)
        {
            try
            {
                Bitmap healthBar = new Bitmap(imageWidth, imageHeight);
                Graphics graphics = createGraphicsObject(healthBar);
                Image profilePic = getProfileImage(boss.ID);
                Image borderImage = getBorderImage();
                float fillAmount = calculateFillAmount(boss.HP, boss.HPMax);
                graphics = drawBackgroundGradient(graphics);
                graphics = drawHealthBar(graphics, fillAmount);
                graphics.DrawImage(borderImage, borderXPos, borderYPos);
                graphics.DrawImage(profilePic, profileXPos, profileYPos);
                graphics = drawNameAndLevel(graphics, boss);
                if (boss.HP <= 0)
                {
                    graphics = drawCrossOnProfilePicture(graphics);
                }
                healthBar.Save(path + "..\\CurrentBossHealthBar.png");
                graphics.Dispose();
                healthBar.Dispose();
            }
            catch
            {

            }
        }

        private Graphics createGraphicsObject(Bitmap image)
        {
            Graphics graphics = Graphics.FromImage(image);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            return graphics;
        }

        private Image getProfileImage(string id)
        {
            try
            {
                Dictionary<string, string> profilePaths = io.LoadBossProfilePicturePaths();
                string filePath = profilePaths[id];
                Image fullProfilePic = Image.FromFile(filePath);
                Image profilePic = new Bitmap(fullProfilePic, profileImageHeight, profileImageWidth);
                fullProfilePic.Dispose();
                return profilePic;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private Image getBorderImage()
        {
            try
            {
                return Image.FromFile(path + "\\..\\BossHealthBarBorder.png");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private float calculateFillAmount(int amount, int total)
        {
            return amount / (float)total;
        }

        private Graphics drawHealthBar(Graphics graphics, float fillAmount)
        {
            SolidBrush brush = new SolidBrush(Color.DarkRed);
            float widthFloat = barWidth * fillAmount;
            int widthInt = (int)widthFloat;
            Rectangle healthBar = new Rectangle(barXPos, barYPos, widthInt, barHeight);
            graphics.FillRectangle(brush, healthBar);
            return graphics;
        }

        private Graphics drawBackgroundGradient(Graphics graphics)// + (barHeight / 2))
        {
            LinearGradientBrush linBrush = new LinearGradientBrush(
                new Point(borderXPos, borderYPos),
                new Point(imageWidth, borderYPos),
                Color.FromArgb(255, 20, 0, 0),
                Color.FromArgb(255, 75, 0, 0));
            graphics.FillRectangle(linBrush, barXPos, barYPos, imageWidth - borderXPos, barHeight);
            linBrush.Dispose();
            return graphics;
        }

        private Graphics drawNameAndLevel(Graphics graphics, Monster boss)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string name = boss.Name.Replace("_", " ").ToLower();
            name = textInfo.ToTitleCase(name);
            string toWrite = name + " lvl:" + boss.Level;
            graphics.DrawString(toWrite, font, Brushes.DeepSkyBlue, textXPos, textYPos);
            return graphics;
        }

        private Graphics drawCrossOnProfilePicture(Graphics graphics)
        {
            Pen redPen = new Pen(Color.Red, 50f);
            PointF point1 = new PointF(-50f, -50f);
            PointF point2 = new PointF(550f, 550f);
            PointF point3 = new PointF(-50f, 550f);
            PointF point4 = new PointF(550f, -50f);
            graphics.DrawLine(redPen, point1, point2);
            graphics.DrawLine(redPen, point3, point4);
            return graphics;
        }

        //// Defines pen 
        //Pen pen = new Pen(ForeColor);

        //// Defines the both points to connect 
        //// pt1 is (30.0, 30.0) which represents (x1, y1) 
        //PointF pt1 = new PointF(30.0F, 30.0F);

        //// pt2 is (200.0, 300.0) which represents (x2, y2) 
        //PointF pt2 = new PointF(200.0F, 300.0F);

        //// Draws the line 
        //pea.Graphics.DrawLine(pen, pt1, pt2); 
    }
}
