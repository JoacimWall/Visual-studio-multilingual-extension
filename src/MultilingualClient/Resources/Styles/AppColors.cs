
namespace MultilingualClient.Resources.Styles;
public static class AppColors
{
    public static void Init(Dictionary<TmColors, Color> colorConfig)
    {
        foreach (var item in colorConfig)
        {
            switch (item.Key)
            {
                //Primary colors
                case TmColors.Primary800Color:
                    Primary800Color = item.Value;
                    break;
                case TmColors.Primary500Color:
                    Primary500Color = item.Value;
                    break;
                case TmColors.Primary400Color:
                    Primary400Color = item.Value;
                    break;
                case TmColors.Primary200Color:
                    Primary200Color = item.Value;
                    break;
                case TmColors.Primary100Color:
                    Primary100Color = item.Value;
                    Primary100Brush = Primary100Color;
                    break;
                case TmColors.PrimaryAccentColor:
                    PrimaryAccentColor = item.Value;
                    AccentBrush = PrimaryAccentColor;
                    break;
                //Signal Colors
                case TmColors.SignalGreenColor:
                    SignalGreenColor = item.Value;
                    break;
                case TmColors.SignalYellowColor:
                    SignalYellowColor = item.Value;
                    break;
                case TmColors.SignalRedColor:
                    SignalRedColor = item.Value;
                    break;
                case TmColors.SignalBlueColor:
                    SignalBlueColor = item.Value;
                    break;
                case TmColors.SignalOrangeColor:
                    SignalOrangeColor = item.Value;
                    break;
                //Base colors
                case TmColors.BlackColor:
                    BlackColor = item.Value;
                    break;
                case TmColors.WhiteColor:
                    WhiteColor = item.Value;
                    WhiteBrush = WhiteColor;
                    break;
                case TmColors.TransparentColor:
                    TransparentColor = item.Value;
                    break;
                case TmColors.SuccessColor:
                    SuccessColor = item.Value;
                    break;
                //Gray colors
                case TmColors.Gray900Color:
                    Gray900Color = item.Value;
                    break;
                case TmColors.Gray700Color:
                    Gray700Color = item.Value;
                    break;
                case TmColors.Gray400Color:
                    Gray400Color = item.Value;
                    break;
                case TmColors.Gray300Color:
                    Gray300Color = item.Value;
                    break;
                case TmColors.Gray200Color:
                    Gray200Color = item.Value;
                    break;
                case TmColors.Gray100Color:
                    Gray100Color = item.Value;
                    break;

                case TmColors.Cyan500Color:
                    Cyan500Color = item.Value;
                    break;
                case TmColors.Red500Color:
                    Red500Color = item.Value;
                    break;
                case TmColors.Gold500Color:
                    Gold500Color = item.Value;
                    break;
               
                //Primary colors
                case TmColors.TemplateTaskButtonTextColor:
                    TemplateTaskButtonTextColor = item.Value;
                    break;
                case TmColors.PageBackgroundColor:
                    PageBackgroundColor = item.Value;
                    break;
                case TmColors.StatusbarBackgroundColor:
                    StatusbarBackgroundColor = item.Value;
                    break;
                case TmColors.TmButtonValidBackgroundColor:
                    TmButtonValidBackgroundColor = item.Value;
                    break;
                case TmColors.TmButtonInValidBackgroundColor:
                    TmButtonInValidBackgroundColor = item.Value;
                    break;


            }
        }

    }

    //------------ Base Colors ----------------------------------//
    public static Color Primary800Color = Color.FromArgb("#23115e");
    public static Color Primary500Color = Color.FromArgb("#2B0B98");
    public static Color Primary400Color = Color.FromArgb("#2d05b5");
    public static Color Primary200Color = Color.FromArgb("#3604de");
    public static Color Primary100Color = Color.FromArgb("#3d05f7");


    public static Color PrimaryAccentColor = Color.FromArgb("#FF675C");

    public static Color SignalGreenColor = Color.FromArgb("#2BDB8C");
    public static Color SignalYellowColor = Color.FromArgb("#F5E500");
    public static Color SignalRedColor = Color.FromArgb("#FF675C");
    public static Color SignalBlueColor = Color.FromArgb("#005EAB");
    public static Color SignalOrangeColor = Color.FromArgb("#F4A528");


    public static Color BlackColor = Color.FromArgb("#000000");
    public static Color WhiteColor = Color.FromArgb("#ffffff");

    public static Color Gray900Color = Color.FromArgb("#1A1A1A");
    public static Color Gray700Color = Color.FromArgb("#878787");
    public static Color Gray400Color = Color.FromArgb("#BBBBBB");
    public static Color Gray300Color = Color.FromArgb("#DEDEDE");
    public static Color Gray200Color = Color.FromArgb("#EAEAEA");
    public static Color Gray100Color = Color.FromArgb("#FAFAFA");

    public static Color Cyan500Color = Color.FromArgb("#19D5E5");
    public static Color Red500Color = Color.FromArgb("#E11D16");
    public static Color Gold500Color = Color.FromArgb("#BA9748");

    public static Color SuccessColor = Color.FromArgb("#3D9642");

    public static Color TransparentColor = Color.FromRgba(0, 0, 0, 0);

    //---------------------------------------------------------------//
    //---------------- Implementation of Brush ---------------------//
    public static Brush Primary100Brush = Color.FromArgb("#FF675C");
    public static Brush WhiteBrush = Color.FromArgb("#ffffff");
    public static Brush AccentBrush = Color.FromArgb("#FF675C");
    //----------------  Controls colors check TietoEvryMauiStyles.xaml  ----------------------//
    //Här ska vi endast lägga till ifall det är så att det inte går att
    //ha detta i en style i ovan nämnda fil;
    public static Color TemplateTaskButtonTextColor = Color.FromArgb("#000000");
    public static Color PageBackgroundColor = Color.FromArgb("#ffffff");
    public static Color DefaultTextColor = Color.FromArgb("#000000");
    public static Color StatusbarBackgroundColor = Color.FromArgb("#2B0B98");
    public static Color StatusbarTextColor = Color.FromArgb("#000000");
    public static Color TmButtonValidBackgroundColor = Color.FromArgb("#FF675C");
    public static Color TmButtonInValidBackgroundColor = Color.FromArgb("#FF675C");
}
public enum TmColors
{
    Primary800Color,
    Primary500Color,
    Primary400Color,
    Primary200Color,
    Primary100Color,

    PrimaryAccentColor,

    SignalGreenColor,
    SignalYellowColor,
    SignalRedColor,
    SignalBlueColor,
    SignalOrangeColor,

    BlackColor,
    WhiteColor,
    SuccessColor,
    TransparentColor,

    Gray900Color,
    Gray700Color,
    Gray400Color,
    Gray300Color,
    Gray200Color,
    Gray100Color,


    Cyan500Color,
    Red500Color,
    Gold500Color,

    Primary100Brush,
    WhiteBrush,
    AccentBrush,

    TemplateTaskButtonTextColor,
    PageBackgroundColor,
    DefaultTextColor,
    StatusbarBackgroundColor,
    StatusbarTextColor,
    TmButtonValidBackgroundColor,
    TmButtonInValidBackgroundColor
}
