using System.ComponentModel;
using Microsoft.SemanticKernel;
using Busylight;
using System.ComponentModel.DataAnnotations;

namespace Plugins;

public class LightPlugin
{
  static SDK? sdk = null;

  [KernelFunction, Description("Turn on the light with the required colour")]
  public static void LightOn(
      [Description("The percentage red colour (from 0 to 100)"), Required] int red,
      [Description("The percentage green colour (from 0 to 100)"), Required] int green,
      [Description("The percentage blue colour (from 0 to 100)"), Required] int blue
  )
  {
    if(sdk == null) sdk = new Busylight.SDK(true);
    // there is a design error in the SDK, the colours are in the wrong order
    sdk.Light(red, blue, green);
  }

  [KernelFunction, Description("Turn off the light")]
  public static void LightOff()
  {
    if (sdk == null) sdk = new Busylight.SDK(true);
    sdk.Light(0, 0, 0);
  }

  [KernelFunction, Description("Make the light flash")]
  public static void LightFlash(
      [Description("The percentage red colour (from 0 to 100)"), Required] int red,
      [Description("The percentage green colour (from 0 to 100)"), Required] int green,
      [Description("The percentage blue colour (from 0 to 100)"), Required] int blue,
      [Description("The on time, in number of tenths of a second (from 0 to 255)")] int onTime=2,
      [Description("The off time, in number of tenths of a second (from 0 to 255)")] int offTime=2
    )
  {
    if (sdk == null) sdk = new Busylight.SDK(true);
    // there is a design error in the SDK, the colours are in the wrong order
    sdk.Blink(red, blue, green, onTime, offTime);
  }
}