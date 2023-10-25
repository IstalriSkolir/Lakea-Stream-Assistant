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
        private int textYPos = 0;
        private int borderThickness = 25;
        private int buffer = 5;
        private int barHeight = 200;
        private int barWidth;
        private int barXPos;
        private int barYPos;

        public HealthBarImage(IO io, string config)
        {
            this.io = io;
            barWidth = imageWidth - (profileImageWidth + borderThickness - buffer);
            barXPos = profileImageWidth + borderThickness - buffer;
            barYPos = imageHeight - (barHeight + borderThickness - buffer);
            if (config == "LAKEA")
            {
                path = Environment.CurrentDirectory + "\\Applications\\Battle Simulator";
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
                healthBar.Save(path + "\\Output\\CurrentBossHealthBar.png");
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
                return Image.FromFile(path + "\\Resources\\BossHealthBarBorder.png");
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
            SolidBrush brush = new SolidBrush(Color.Red);
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
            graphics.DrawString(toWrite, font, Brushes.Black, textXPos, textYPos);
            return graphics;
        }
    }
}
