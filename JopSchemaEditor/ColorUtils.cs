using Microsoft.Xna.Framework;

namespace JopSchemaEditor
{
    public static class ColorUtils
    {
        /// <summary>
        /// Určí, zda je daná barva více kontrastní s bílou nebo černou.
        /// </summary>
        /// <param name="color">Barva pro výpočet kontrastu.</param>
        /// <returns>True, pokud je barva více kontrastní s bílou, False, pokud s černou (nebo je kontrast s oběma stejný).</returns>
        public static bool IsMoreContrastWithWhite(Color color)
        {
            // Vypočítáme jas (luminance) barvy podle standardu sRGB
            double luminance = (0.2126 * color.R + 0.7152 * color.G + 0.0722 * color.B) / 255.0;

            // Jas bílé je 1.0, jas černé je 0.0

            // Kontrast s bílou je rozdíl mezi jasem bílé a jasem barvy
            double contrastWithWhite = Math.Abs(1.0 - luminance);

            // Kontrast s černou je rozdíl mezi jasem barvy a jasem černé
            double contrastWithBlack = Math.Abs(luminance - 0.0);

            // Porovnáme kontrasty
            return contrastWithWhite > contrastWithBlack;
        }

        /// <summary>
        /// Vrátí barvu (bílou nebo černou), která má větší kontrast s danou barvou.
        /// </summary>
        /// <param name="color">Barva pro výpočet kontrastu.</param>
        /// <returns>Bílá nebo černá barva s větším kontrastem.</returns>
        public static Color GetMoreContrastColor(Color color)
        {
            return IsMoreContrastWithWhite(color) ? Color.White : Color.Black;
        }
    }
}
