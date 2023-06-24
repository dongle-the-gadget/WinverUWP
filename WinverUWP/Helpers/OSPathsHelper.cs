using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WinverUWP.Helpers;

public class FirstDisposableTuple<T1, T2, T3> : Tuple<T1, T2, T3>, IDisposable where T1 : IDisposable
{
    public FirstDisposableTuple(T1 item1, T2 item2, T3 item3) : base(item1, item2, item3)
    { }

    public void Dispose() => Item1.Dispose();
}

public static class OSPathsHelper
{
    public static FirstDisposableTuple<CanvasPathBuilder, float, float> GetWindows11Path()
    {
        var pathBuilder = new CanvasPathBuilder(null);

        pathBuilder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);

        pathBuilder.BeginFigure(new Vector2(264.58f, 48.746f));
        pathBuilder.EndFigure(CanvasFigureLoop.Closed);

        pathBuilder.BeginFigure(new Vector2(0f, 0f));
        pathBuilder.EndFigure(CanvasFigureLoop.Closed);

        pathBuilder.BeginFigure(new Vector2(264.5832f, 9.017f));
        pathBuilder.AddLine(new Vector2(264.5832f, 38.885f));
        pathBuilder.AddLine(new Vector2(259.5938f, 38.885f));
        pathBuilder.AddLine(new Vector2(259.5938f, 15.06f));
        pathBuilder.AddCubicBezier(new Vector2(259.5938f, 15.06f), new Vector2(257.8831f, 16.6403f), new Vector2(253.4923f, 17.7854f));
        pathBuilder.AddLine(new Vector2(253.4923f, 13.5338f));
        pathBuilder.AddCubicBezier(new Vector2(260.018f, 11.1204f), new Vector2(262.3605f, 9.0173f), new Vector2(262.3605f, 9.0173f));
        pathBuilder.EndFigure(CanvasFigureLoop.Closed);

        pathBuilder.BeginFigure(new Vector2(25.64f, 25.642f));
        pathBuilder.AddLine(new Vector2(48.745f, 25.642f));
        pathBuilder.AddLine(new Vector2(48.745f, 48.747f));
        pathBuilder.AddLine(new Vector2(25.64f, 48.747f));
        pathBuilder.EndFigure(CanvasFigureLoop.Closed);

        pathBuilder.BeginFigure(new Vector2(0f, 25.642f));
        pathBuilder.AddLine(new Vector2(23.105f, 25.642f));
        pathBuilder.AddLine(new Vector2(23.105f, 48.747f));
        pathBuilder.AddLine(new Vector2(0f, 48.747f));
        pathBuilder.EndFigure(CanvasFigureLoop.Closed);

        pathBuilder.BeginFigure(new Vector2(25.64f, 0f));
        pathBuilder.AddLine(new Vector2(48.745f, 0f));
        pathBuilder.AddLine(new Vector2(48.745f, 23.105f));
        pathBuilder.AddLine(new Vector2(25.64f, 23.105f));
        pathBuilder.EndFigure(CanvasFigureLoop.Closed);

        pathBuilder.BeginFigure(new Vector2(0f, 0f));
        pathBuilder.AddLine(new Vector2(23.105f, 0f));
        pathBuilder.AddLine(new Vector2(23.105f, 23.105f));
        pathBuilder.AddLine(new Vector2(0f, 23.105f));
        pathBuilder.EndFigure(CanvasFigureLoop.Closed);

        pathBuilder.BeginFigure(new Vector2(249.849f, 9.017f));
        pathBuilder.AddLine(new Vector2(249.849f, 38.885f));
        pathBuilder.AddLine(new Vector2(244.8596f, 38.885f));
        pathBuilder.AddLine(new Vector2(244.8596f, 15.06f));
        pathBuilder.AddCubicBezier(new Vector2(244.8596f, 15.06f), new Vector2(243.1489f, 16.6403f), new Vector2(238.7581f, 17.7854f));
        pathBuilder.AddLine(new Vector2(238.7581f, 13.5338f));
        pathBuilder.AddCubicBezier(new Vector2(245.2838f, 11.1204f), new Vector2(247.6263f, 9.0173f), new Vector2(247.6263f, 9.0173f));
        pathBuilder.EndFigure(CanvasFigureLoop.Closed);

        pathBuilder.BeginFigure(new Vector2(230.665f, 33.063f));
        pathBuilder.AddCubicBezier(new Vector2(230.665f, 37.7195f), new Vector2(226.4425f, 39.5344f), new Vector2(222.2096f, 39.5344f));
        pathBuilder.AddCubicBezier(new Vector2(219.4527f, 39.5344f), new Vector2(216.7077f, 38.4725f), new Vector2(216.7077f, 38.4725f));
        pathBuilder.AddLine(new Vector2(216.7077f, 33.5196f));
        pathBuilder.AddCubicBezier(new Vector2(216.7077f, 33.5196f), new Vector2(219.0938f, 35.5989f), new Vector2(222.4269f, 35.6248f));
        pathBuilder.AddCubicBezier(new Vector2(224.7118f, 35.6248f), new Vector2(225.7411f, 34.90423f), new Vector2(225.7411f, 33.5324f));
        pathBuilder.AddCubicBezier(new Vector2(225.7975f, 32.4318f), new Vector2(225.1275f, 31.8249f), new Vector2(224.3264f, 31.3699f));
        pathBuilder.AddCubicBezier(new Vector2(223.7758f, 31.03955f), new Vector2(222.9155f, 30.63349f), new Vector2(221.7455f, 30.1517f));
        pathBuilder.AddCubicBezier(new Vector2(220.369f, 29.55983f), new Vector2(219.3436f, 28.9886f), new Vector2(218.6691f, 28.438f));
        pathBuilder.AddCubicBezier(new Vector2(218.0084f, 27.88742f), new Vector2(217.5129f, 27.2405f), new Vector2(217.1825f, 26.4972f));
        pathBuilder.AddCubicBezier(new Vector2(216.8659f, 25.75391f), new Vector2(216.6322f, 24.9371f), new Vector2(216.6322f, 23.946f));
        pathBuilder.AddCubicBezier(new Vector2(216.6322f, 20.186f), new Vector2(219.7681f, 17.5293f), new Vector2(224.7807f, 17.5293f));
        pathBuilder.AddCubicBezier(new Vector2(228.0936f, 17.5293f), new Vector2(229.5294f, 18.34169f), new Vector2(229.5294f, 18.34169f));
        pathBuilder.AddLine(new Vector2(229.5294f, 23.04689f));
        pathBuilder.AddCubicBezier(new Vector2(229.5294f, 23.04689f), new Vector2(227.175f, 21.41969f), new Vector2(224.7287f, 21.40489f));
        pathBuilder.AddCubicBezier(new Vector2(222.9758f, 21.40489f), new Vector2(221.6503f, 22.04309f), new Vector2(221.627f, 23.48739f));
        pathBuilder.AddCubicBezier(new Vector2(221.6147f, 25.34899f), new Vector2(223.8943f, 26.25639f), new Vector2(225.2554f, 26.80709f));
        pathBuilder.AddCubicBezier(new Vector2(227.2375f, 27.60544f), new Vector2(228.688f, 28.45769f), new Vector2(229.5002f, 29.43499f));
        pathBuilder.AddCubicBezier(new Vector2(230.3123f, 30.41229f), new Vector2(230.6649f, 31.39639f), new Vector2(230.6649f, 33.06319f));
        pathBuilder.EndFigure(CanvasFigureLoop.Closed);

        pathBuilder.BeginFigure(new Vector2(209.356f, 38.885f));
        pathBuilder.AddLine(new Vector2(203.9749f, 38.885f));
        pathBuilder.AddLine(new Vector2(199.8616f, 23.748f));
        pathBuilder.AddLine(new Vector2(195.4489f, 38.885f));
        pathBuilder.AddLine(new Vector2(190.2259f, 38.885f));
        pathBuilder.AddLine(new Vector2(184.1413f, 18.034f));
        pathBuilder.AddLine(new Vector2(189.2475f, 18.034f));
        pathBuilder.AddLine(new Vector2(193.221f, 34.233f));
        pathBuilder.AddLine(new Vector2(197.8651f, 18.034f));
        pathBuilder.AddLine(new Vector2(202.6545f, 18.034f));
        pathBuilder.AddLine(new Vector2(206.7953f, 34.191f));
        pathBuilder.AddLine(new Vector2(210.6688f, 18.034f));
        pathBuilder.AddLine(new Vector2(215.3485f, 18.034f));
        pathBuilder.EndFigure(CanvasFigureLoop.Closed);

        pathBuilder.BeginFigure(new Vector2(183.186f, 28.335f));
        pathBuilder.AddCubicBezier(new Vector2(183.186f, 35.5796f), new Vector2(178.8562f, 39.504f), new Vector2(172.354f, 39.504f));
        pathBuilder.AddCubicBezier(new Vector2(165.2833f, 39.504f), new Vector2(161.659f, 35.2712f), new Vector2(161.659f, 28.789f));
        pathBuilder.AddCubicBezier(new Vector2(161.659f, 21.3733f), new Vector2(166.0019f, 17.516f), new Vector2(172.87f, 17.516f));
        pathBuilder.AddCubicBezier(new Vector2(179.1632f, 17.516f), new Vector2(183.186f, 21.5388f), new Vector2(183.186f, 28.335f));
        pathBuilder.EndFigure(CanvasFigureLoop.Closed);

        pathBuilder.BeginFigure(new Vector2(178.1066f, 28.5001f));
        pathBuilder.AddCubicBezier(new Vector2(178.1066f, 25.277f), new Vector2(176.9641f, 21.6942f), new Vector2(172.6015f, 21.6942f));
        pathBuilder.AddCubicBezier(new Vector2(168.4173f, 21.6942f), new Vector2(166.7918f, 24.808f), new Vector2(166.7918f, 28.624f));
        pathBuilder.AddCubicBezier(new Vector2(166.7918f, 32.7696f), new Vector2(168.7442f, 35.4064f), new Vector2(172.5717f, 35.4064f));
        pathBuilder.AddCubicBezier(new Vector2(176.6722f, 35.4064f), new Vector2(178.0768f, 32.2593f), new Vector2(178.1067f, 28.5001f));
        pathBuilder.EndFigure(CanvasFigureLoop.Closed);

        pathBuilder.BeginFigure(new Vector2(153.28f, 38.885f));
        pathBuilder.AddLine(new Vector2(153.28f, 36.1926f));
        pathBuilder.AddLine(new Vector2(153.1974f, 36.1926f));
        pathBuilder.AddCubicBezier(new Vector2(151.6833f, 38.7391f), new Vector2(148.5926f, 39.3262f), new Vector2(146.4378f, 39.3262f));
        pathBuilder.AddCubicBezier(new Vector2(140.0336f, 39.3262f), new Vector2(137.6835f, 34.3513f), new Vector2(137.6835f, 29.0572f));
        pathBuilder.AddCubicBezier(new Vector2(137.6835f, 25.5472f), new Vector2(138.5576f, 22.7392f), new Vector2(140.3057f, 20.6332f));
        pathBuilder.AddCubicBezier(new Vector2(142.0676f, 18.5134f), new Vector2(144.4213f, 17.5959f), new Vector2(147.367f, 17.5959f));
        pathBuilder.AddCubicBezier(new Vector2(151.9525f, 17.5959f), new Vector2(153.1975f, 20.1472f), new Vector2(153.1975f, 20.1472f));
        pathBuilder.AddLine(new Vector2(153.2801f, 20.1472f));
        pathBuilder.AddLine(new Vector2(153.2801f, 8.0432f));
        pathBuilder.AddLine(new Vector2(158.2274f, 8.0432f));
        pathBuilder.AddLine(new Vector2(158.2274f, 38.8852f));
        pathBuilder.EndFigure(CanvasFigureLoop.Closed);

        pathBuilder.BeginFigure(new Vector2(153.3213f, 26.967f));
        pathBuilder.AddCubicBezier(new Vector2(153.3213f, 24.4405f), new Vector2(151.6977f, 21.57f), new Vector2(148.2709f, 21.57f));
        pathBuilder.AddCubicBezier(new Vector2(144.3509f, 21.57f), new Vector2(142.8275f, 24.9813f), new Vector2(142.8275f, 28.913f));
        pathBuilder.AddCubicBezier(new Vector2(142.8275f, 32.3456f), new Vector2(144.265f, 35.461f), new Vector2(147.998f, 35.5127f));
        pathBuilder.AddCubicBezier(new Vector2(151.6584f, 35.5127f), new Vector2(153.2916f, 32.024f), new Vector2(153.3213f, 29.1401f));
        pathBuilder.EndFigure(CanvasFigureLoop.Closed);

        pathBuilder.BeginFigure(new Vector2(129.33f, 38.885f));
        pathBuilder.AddLine(new Vector2(129.33f, 27.24f));
        pathBuilder.AddCubicBezier(new Vector2(129.33f, 24.1889f), new Vector2(128.3836f, 21.5569f), new Vector2(125.3099f, 21.5569f));
        pathBuilder.AddCubicBezier(new Vector2(122.2522f, 21.5569f), new Vector2(120.3133f, 24.3102f), new Vector2(120.3133f, 27.0955f));
        pathBuilder.AddLine(new Vector2(120.3133f, 38.8845f));
        pathBuilder.AddLine(new Vector2(115.5438f, 38.8845f));
        pathBuilder.AddLine(new Vector2(115.5438f, 17.9695f));
        pathBuilder.AddLine(new Vector2(120.3133f, 17.9695f));
        pathBuilder.AddLine(new Vector2(120.3133f, 20.9279f));
        pathBuilder.AddLine(new Vector2(120.3959f, 20.9279f));
        pathBuilder.AddCubicBezier(new Vector2(121.9788f, 18.4773f), new Vector2(124.2638f, 17.6168f), new Vector2(127.2507f, 17.6168f));
        pathBuilder.AddCubicBezier(new Vector2(129.4943f, 17.6168f), new Vector2(131.2287f, 18.16905f), new Vector2(132.4537f, 19.6006f));
        pathBuilder.AddCubicBezier(new Vector2(133.6925f, 21.0321f), new Vector2(134.2364f, 23.2069f), new Vector2(134.2364f, 26.1251f));
        pathBuilder.AddLine(new Vector2(134.2364f, 38.8841f));
        pathBuilder.EndFigure(CanvasFigureLoop.Closed);

        pathBuilder.BeginFigure(new Vector2(111.13f, 11.986f));
        pathBuilder.AddCubicBezier(new Vector2(111.13f, 12.7706f), new Vector2(110.8409f, 13.4244f), new Vector2(110.2628f, 13.9475f));
        pathBuilder.AddCubicBezier(new Vector2(109.6985f, 14.47056f), new Vector2(109.0102f, 14.73209f), new Vector2(108.1981f, 14.73209f));
        pathBuilder.AddCubicBezier(new Vector2(107.386f, 14.73209f), new Vector2(106.6977f, 14.47053f), new Vector2(106.1334f, 13.9475f));
        pathBuilder.AddCubicBezier(new Vector2(105.5691f, 13.42443f), new Vector2(105.2869f, 12.7706f), new Vector2(105.2869f, 11.986f));
        pathBuilder.AddCubicBezier(new Vector2(105.2869f, 11.18766f), new Vector2(105.5691f, 10.5201f), new Vector2(106.1334f, 9.9832f));
        pathBuilder.AddCubicBezier(new Vector2(106.7115f, 9.44638f), new Vector2(107.3998f, 9.17796f), new Vector2(108.1981f, 9.17796f));
        pathBuilder.AddCubicBezier(new Vector2(109.0378f, 9.17796f), new Vector2(109.7329f, 9.4533f), new Vector2(110.2835f, 10.00384f));
        pathBuilder.AddCubicBezier(new Vector2(110.8479f, 10.55446f), new Vector2(111.1301f, 11.21514f), new Vector2(111.1301f, 11.98594f));
        pathBuilder.EndFigure(CanvasFigureLoop.Closed);

        pathBuilder.BeginFigure(new Vector2(105.688f, 38.884f));
        pathBuilder.AddLine(new Vector2(105.688f, 17.969f));
        pathBuilder.AddLine(new Vector2(110.564f, 17.969f));
        pathBuilder.AddLine(new Vector2(110.564f, 38.884f));
        pathBuilder.EndFigure(CanvasFigureLoop.Closed);

        pathBuilder.BeginFigure(new Vector2(95.55f, 38.885f));
        pathBuilder.AddLine(new Vector2(89.5343f, 38.885f));
        pathBuilder.AddLine(new Vector2(83.6685f, 16.557f));
        pathBuilder.AddLine(new Vector2(77.7381f, 38.885f));
        pathBuilder.AddLine(new Vector2(71.5895f, 38.885f));
        pathBuilder.AddLine(new Vector2(63.5242f, 9.863f));
        pathBuilder.AddLine(new Vector2(68.9805f, 9.863f));
        pathBuilder.AddLine(new Vector2(74.7966f, 32.948f));
        pathBuilder.AddLine(new Vector2(81.1462f, 9.863f));
        pathBuilder.AddLine(new Vector2(86.6574f, 9.863f));
        pathBuilder.AddLine(new Vector2(92.6257f, 33.085f));
        pathBuilder.AddLine(new Vector2(98.1495f, 9.863f));
        pathBuilder.AddLine(new Vector2(103.418f, 9.863f));
        pathBuilder.EndFigure(CanvasFigureLoop.Closed);

        return new(pathBuilder, 264.58319091796875f, 48.747001647949219f);
    }

    public static FirstDisposableTuple<CanvasPathBuilder, float, float> GetWindows10Path()
    {
        var pathBuilder = new CanvasPathBuilder(null);

        pathBuilder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);

        pathBuilder.BeginFigure(new Vector2(0f, 12.5f));
        pathBuilder.AddLine(new Vector2(35.6973f, 7.59863f));
        pathBuilder.AddLine(new Vector2(35.6973f, 42.0986f));
        pathBuilder.AddLine(new Vector2(0f, 42.0986f));
        pathBuilder.EndFigure(CanvasFigureLoop.Open);

        pathBuilder.BeginFigure(new Vector2(40f, 6.90131f));
        pathBuilder.AddLine(new Vector2(87.3027f, 0f));
        pathBuilder.AddLine(new Vector2(87.3027f, 41.8027f));
        pathBuilder.AddLine(new Vector2(40f, 41.8027f));
        pathBuilder.EndFigure(CanvasFigureLoop.Open);

        pathBuilder.BeginFigure(new Vector2(0f, 45.74f));
        pathBuilder.AddLine(new Vector2(35.6973f, 45.74f));
        pathBuilder.AddLine(new Vector2(35.6973f, 80.3386f));
        pathBuilder.AddLine(new Vector2(0f, 75.3386f));
        pathBuilder.EndFigure(CanvasFigureLoop.Open);

        pathBuilder.BeginFigure(new Vector2(40f, 46.1973f));
        pathBuilder.AddLine(new Vector2(87.3027f, 46.1973f));
        pathBuilder.AddLine(new Vector2(87.3027f, 87.5986f));
        pathBuilder.AddLine(new Vector2(40f, 80.9013f));
        pathBuilder.EndFigure(CanvasFigureLoop.Open);

        pathBuilder.BeginFigure(new Vector2(114f, 17f));
        pathBuilder.AddLine(new Vector2(120.76f, 17.0413f));
        pathBuilder.AddLine(new Vector2(131.26f, 55.14f));
        pathBuilder.AddCubicBezier(new Vector2(132.011f, 57.8027f), new Vector2(132.563f, 60.5213f), new Vector2(132.901f, 63.2813f));
        pathBuilder.AddCubicBezier(new Vector2(136.563f, 47.6826f), new Vector2(141.563f, 32.4786f), new Vector2(145.803f, 17.0787f));
        pathBuilder.AddLine(new Vector2(151.651f, 17.0986f));
        pathBuilder.AddLine(new Vector2(160.688f, 50.0986f));
        pathBuilder.AddCubicBezier(new Vector2(161.817f, 54.4786f), new Vector2(163.292f, 58.8027f), new Vector2(163.74f, 63.3027f));
        pathBuilder.AddCubicBezier(new Vector2(167.041f, 47.6973f), new Vector2(171.771f, 32.5f), new Vector2(175.541f, 17.0986f));
        pathBuilder.AddLine(new Vector2(182.197f, 17.068f));
        pathBuilder.AddLine(new Vector2(167.401f, 70.068f));
        pathBuilder.AddLine(new Vector2(160.197f, 70.068f));
        pathBuilder.AddLine(new Vector2(149.5f, 31.9693f));
        pathBuilder.AddCubicBezier(new Vector2(148.901f, 29.9213f), new Vector2(148.651f, 27.8027f), new Vector2(148.459f, 25.672f));
        pathBuilder.AddCubicBezier(new Vector2(148.183f, 27.5f), new Vector2(147.921f, 29.3493f), new Vector2(147.459f, 31.14f));
        pathBuilder.AddLine(new Vector2(136.661f, 69.9373f));
        pathBuilder.AddCubicBezier(new Vector2(134.26f, 70.068f), new Vector2(131.86f, 70.0787f), new Vector2(129.459f, 70.0626f));
        pathBuilder.AddLine(new Vector2(114.063f, 17.0626f));
        pathBuilder.EndFigure(CanvasFigureLoop.Open);

        pathBuilder.BeginFigure(new Vector2(186f, 17.4013f));
        pathBuilder.AddCubicBezier(new Vector2(186.265f, 15.3066f), new Vector2(188.109f, 13.776f), new Vector2(190.219f, 13.9066f));
        pathBuilder.AddCubicBezier(new Vector2(192.328f, 14.0413f), new Vector2(193.969f, 15.7866f), new Vector2(193.969f, 17.9013f));
        pathBuilder.AddCubicBezier(new Vector2(193.969f, 20.0106f), new Vector2(192.328f, 21.7599f), new Vector2(190.219f, 21.8906f));
        pathBuilder.AddCubicBezier(new Vector2(188.109f, 22.0267f), new Vector2(186.265f, 20.4946f), new Vector2(186f, 18.4013f));
        pathBuilder.EndFigure(CanvasFigureLoop.Open);

        pathBuilder.BeginFigure(new Vector2(186.901f, 32f));
        pathBuilder.AddLine(new Vector2(192.901f, 32f));
        pathBuilder.AddLine(new Vector2(192.901f, 70f));
        pathBuilder.AddLine(new Vector2(186.901f, 70f));
        pathBuilder.EndFigure(CanvasFigureLoop.Open);

        pathBuilder.BeginFigure(new Vector2(209.063f, 38.12f));
        pathBuilder.AddCubicBezier(new Vector2(212.489f, 32.0213f), new Vector2(220.489f, 29.1973f), new Vector2(226.937f, 32.0213f));
        pathBuilder.AddCubicBezier(new Vector2(232.068f, 34.0213f), new Vector2(233.979f, 39.9586f), new Vector2(234.14f, 45.0213f));
        pathBuilder.AddCubicBezier(new Vector2(234.421f, 53.3386f), new Vector2(234.188f, 61.672f), new Vector2(234.26f, 69.9893f));
        pathBuilder.AddCubicBezier(new Vector2(232.26f, 69.9893f), new Vector2(230.26f, 69.9893f), new Vector2(228.26f, 69.9786f));
        pathBuilder.AddCubicBezier(new Vector2(228.161f, 61.9786f), new Vector2(228.459f, 53.9786f), new Vector2(228.12f, 45.9786f));
        pathBuilder.AddCubicBezier(new Vector2(227.959f, 42.0213f), new Vector2(226.281f, 37.2599f), new Vector2(221.969f, 36.2507f));
        pathBuilder.AddCubicBezier(new Vector2(215.672f, 34.3493f), new Vector2(209.401f, 40.0213f), new Vector2(209.197f, 46.2507f));
        pathBuilder.AddCubicBezier(new Vector2(209.021f, 54.1506f), new Vector2(209.188f, 62.0787f), new Vector2(209.131f, 69.9893f));
        pathBuilder.AddLine(new Vector2(203.131f, 69.9893f));
        pathBuilder.AddLine(new Vector2(203.131f, 32f));
        pathBuilder.AddLine(new Vector2(209.131f, 32f));
        pathBuilder.AddLine(new Vector2(209.052f, 38.12f));
        pathBuilder.EndFigure(CanvasFigureLoop.Open);

        pathBuilder.BeginFigure(new Vector2(270f, 14f));
        pathBuilder.AddLine(new Vector2(276f, 14f));
        pathBuilder.AddLine(new Vector2(276f, 70f));
        pathBuilder.AddLine(new Vector2(270.021f, 70f));
        pathBuilder.AddLine(new Vector2(269.849f, 63.9013f));
        pathBuilder.AddCubicBezier(new Vector2(265.521f, 71.6826f), new Vector2(253.948f, 73.4013f), new Vector2(247.349f, 67.6306f));
        pathBuilder.AddCubicBezier(new Vector2(242.62f, 63.5626f), new Vector2(241.183f, 56.9319f), new Vector2(241.432f, 50.9319f));
        pathBuilder.AddCubicBezier(new Vector2(241.479f, 44.8493f), new Vector2(243.479f, 38.4319f), new Vector2(248.328f, 34.4319f));
        pathBuilder.AddCubicBezier(new Vector2(254.631f, 29.2507f), new Vector2(265.729f, 29.552f), new Vector2(270.131f, 37.1613f));
        pathBuilder.AddLine(new Vector2(270.088f, 13.9586f));
        pathBuilder.EndFigure(CanvasFigureLoop.Open);

        pathBuilder.BeginFigure(new Vector2(257.188f, 36.0626f));
        pathBuilder.AddCubicBezier(new Vector2(252.391f, 36.8173f), new Vector2(249.14f, 41.1973f), new Vector2(248.24f, 45.7293f));
        pathBuilder.AddCubicBezier(new Vector2(247.063f, 51.188f), new Vector2(247.271f, 57.5306f), new Vector2(250.708f, 62.1306f));
        pathBuilder.AddCubicBezier(new Vector2(254.432f, 67.068f), new Vector2(262.609f, 67.1826f), new Vector2(266.609f, 62.5106f));
        pathBuilder.AddCubicBezier(new Vector2(269.312f, 59.688f), new Vector2(270.088f, 55.6506f), new Vector2(270.109f, 51.912f));
        pathBuilder.AddCubicBezier(new Vector2(270.14f, 47.912f), new Vector2(270.479f, 43.412f), new Vector2(267.828f, 40.1093f));
        pathBuilder.AddCubicBezier(new Vector2(265.489f, 36.7813f), new Vector2(261.131f, 35.2813f), new Vector2(257.229f, 36.1093f));
        pathBuilder.EndFigure(CanvasFigureLoop.Open);

        pathBuilder.BeginFigure(new Vector2(299f, 31.1093f));
        pathBuilder.AddCubicBezier(new Vector2(304.349f, 30.64f), new Vector2(310.303f, 31.4786f), new Vector2(314.303f, 35.3493f));
        pathBuilder.AddCubicBezier(new Vector2(318.839f, 39.552f), new Vector2(320.197f, 46.052f), new Vector2(319.937f, 51.948f));
        pathBuilder.AddCubicBezier(new Vector2(319.869f, 57.3027f), new Vector2(317.891f, 62.7493f), new Vector2(313.969f, 66.448f));
        pathBuilder.AddCubicBezier(new Vector2(305.792f, 73.8906f), new Vector2(290.568f, 72.2507f), new Vector2(285.369f, 62.0306f));
        pathBuilder.AddCubicBezier(new Vector2(281.292f, 53.4586f), new Vector2(282.109f, 41.6307f), new Vector2(289.568f, 35.1307f));
        pathBuilder.AddCubicBezier(new Vector2(292.24f, 32.8693f), new Vector2(295.62f, 31.828f), new Vector2(298.969f, 31.088f));
        pathBuilder.EndFigure(CanvasFigureLoop.Open);

        pathBuilder.BeginFigure(new Vector2(297.891f, 36.5306f));
        pathBuilder.AddCubicBezier(new Vector2(289.088f, 39.2293f), new Vector2(287.588f, 50.7293f), new Vector2(290.339f, 58.328f));
        pathBuilder.AddCubicBezier(new Vector2(292.541f, 65.052f), new Vector2(301.041f, 68.0306f), new Vector2(307.24f, 64.8906f));
        pathBuilder.AddCubicBezier(new Vector2(311.391f, 62.9893f), new Vector2(313.281f, 58.292f), new Vector2(313.683f, 53.9893f));
        pathBuilder.AddCubicBezier(new Vector2(314.131f, 48.9893f), new Vector2(313.729f, 43.292f), new Vector2(310.183f, 39.3906f));
        pathBuilder.AddCubicBezier(new Vector2(307.183f, 36.0106f), new Vector2(302.099f, 35.292f), new Vector2(297.88f, 36.5213f));
        pathBuilder.EndFigure(CanvasFigureLoop.Open);

        pathBuilder.BeginFigure(new Vector2(323f, 32f));
        pathBuilder.AddLine(new Vector2(329.26f, 32f));
        pathBuilder.AddLine(new Vector2(336.661f, 59.1973f));
        pathBuilder.AddCubicBezier(new Vector2(337.161f, 60.9013f), new Vector2(337.36f, 62.6613f), new Vector2(337.541f, 64.4213f));
        pathBuilder.AddCubicBezier(new Vector2(340.369f, 53.5213f), new Vector2(343.937f, 42.8173f), new Vector2(347.161f, 32.0213f));
        pathBuilder.AddLine(new Vector2(352.631f, 32.052f));
        pathBuilder.AddCubicBezier(new Vector2(355.38f, 42.8493f), new Vector2(358.803f, 53.448f), new Vector2(361.079f, 64.448f));
        pathBuilder.AddCubicBezier(new Vector2(363.432f, 53.6506f), new Vector2(366.781f, 42.948f), new Vector2(369.38f, 32.2507f));
        pathBuilder.AddCubicBezier(new Vector2(371.38f, 32.1506f), new Vector2(373.401f, 32.12f), new Vector2(375.421f, 32.0986f));
        pathBuilder.AddLine(new Vector2(364.12f, 70.0986f));
        pathBuilder.AddLine(new Vector2(357.901f, 70.0986f));
        pathBuilder.AddCubicBezier(new Vector2(355.197f, 59.8027f), new Vector2(351.697f, 49.6973f), new Vector2(349.5f, 39.3027f));
        pathBuilder.AddCubicBezier(new Vector2(346.901f, 49.6973f), new Vector2(343.271f, 59.8027f), new Vector2(340.303f, 70.0986f));
        pathBuilder.AddLine(new Vector2(334.303f, 70.0986f));
        pathBuilder.AddLine(new Vector2(323f, 32.0986f));
        pathBuilder.EndFigure(CanvasFigureLoop.Open);

        pathBuilder.BeginFigure(new Vector2(390f, 31f));
        pathBuilder.AddCubicBezier(new Vector2(393.749f, 30.74f), new Vector2(397.568f, 31.0986f), new Vector2(401.099f, 32.4786f));
        pathBuilder.AddCubicBezier(new Vector2(402.079f, 34.3386f), new Vector2(401.317f, 36.7813f), new Vector2(401.552f, 38.88f));
        pathBuilder.AddCubicBezier(new Vector2(397.349f, 36.2507f), new Vector2(391.552f, 34.5787f), new Vector2(386.948f, 37.2507f));
        pathBuilder.AddCubicBezier(new Vector2(383.817f, 38.948f), new Vector2(383.672f, 43.9786f), new Vector2(386.719f, 45.8493f));
        pathBuilder.AddCubicBezier(new Vector2(391.088f, 48.6826f), new Vector2(396.64f, 49.4213f), new Vector2(400.521f, 53.052f));
        pathBuilder.AddCubicBezier(new Vector2(404.803f, 57.172f), new Vector2(403.76f, 65.052f), new Vector2(398.749f, 68.1506f));
        pathBuilder.AddCubicBezier(new Vector2(392.651f, 72.0306f), new Vector2(384.651f, 71.6506f), new Vector2(378.251f, 68.688f));
        pathBuilder.AddLine(new Vector2(378.197f, 61.9693f));
        pathBuilder.AddCubicBezier(new Vector2(382.74f, 65.4319f), new Vector2(389.099f, 67.3493f), new Vector2(394.599f, 64.8906f));
        pathBuilder.AddCubicBezier(new Vector2(397.771f, 63.292f), new Vector2(397.932f, 58.4319f), new Vector2(395.14f, 56.3906f));
        pathBuilder.AddCubicBezier(new Vector2(390.771f, 53.172f), new Vector2(384.74f, 52.6613f), new Vector2(380.937f, 48.64f));
        pathBuilder.AddCubicBezier(new Vector2(377.109f, 44.7813f), new Vector2(377.803f, 37.64f), new Vector2(382.068f, 34.4373f));
        pathBuilder.AddCubicBezier(new Vector2(384.271f, 32.412f), new Vector2(387.271f, 31.8027f), new Vector2(390.068f, 31.0106f));
        pathBuilder.EndFigure(CanvasFigureLoop.Open);

        pathBuilder.BeginFigure(new Vector2(432f, 16.1973f));
        pathBuilder.AddLine(new Vector2(434.099f, 15.9013f));
        pathBuilder.AddLine(new Vector2(434.099f, 70f));
        pathBuilder.AddLine(new Vector2(428.183f, 70f));
        pathBuilder.AddLine(new Vector2(428.251f, 24.9013f));
        pathBuilder.AddCubicBezier(new Vector2(424.479f, 27.5f), new Vector2(420.412f, 29.74f), new Vector2(415.948f, 30.9786f));
        pathBuilder.AddLine(new Vector2(416.021f, 24.9786f));
        pathBuilder.AddCubicBezier(new Vector2(421.817f, 23.0106f), new Vector2(427.12f, 19.828f), new Vector2(431.921f, 16.12f));
        pathBuilder.EndFigure(CanvasFigureLoop.Open);

        pathBuilder.BeginFigure(new Vector2(456f, 16.0986f));
        pathBuilder.AddCubicBezier(new Vector2(461.197f, 15.2293f), new Vector2(467f, 17.1307f), new Vector2(470.099f, 21.552f));
        pathBuilder.AddCubicBezier(new Vector2(474.5f, 27.7493f), new Vector2(474.901f, 35.7493f), new Vector2(475.12f, 43.052f));
        pathBuilder.AddCubicBezier(new Vector2(474.969f, 50.792f), new Vector2(474.12f, 59.1506f), new Vector2(469.183f, 65.448f));
        pathBuilder.AddCubicBezier(new Vector2(464.631f, 71.4213f), new Vector2(455.281f, 72.8173f), new Vector2(449.183f, 68.4319f));
        pathBuilder.AddCubicBezier(new Vector2(444.229f, 64.86f), new Vector2(442.281f, 58.6306f), new Vector2(441.412f, 52.828f));
        pathBuilder.AddCubicBezier(new Vector2(440.412f, 44.2599f), new Vector2(440.511f, 35.2293f), new Vector2(443.479f, 27.1307f));
        pathBuilder.AddCubicBezier(new Vector2(445.36f, 21.5787f), new Vector2(450f, 16.828f), new Vector2(455.979f, 16.1307f));
        pathBuilder.EndFigure(CanvasFigureLoop.Open);

        pathBuilder.BeginFigure(new Vector2(455.063f, 21.5f));
        pathBuilder.AddCubicBezier(new Vector2(449.729f, 23.6613f), new Vector2(448.197f, 29.9586f), new Vector2(447.459f, 35.0986f));
        pathBuilder.AddCubicBezier(new Vector2(446.683f, 42.24f), new Vector2(446.5f, 49.5986f), new Vector2(448.197f, 56.5986f));
        pathBuilder.AddCubicBezier(new Vector2(449.052f, 60.0626f), new Vector2(450.839f, 63.7293f), new Vector2(454.271f, 65.2507f));
        pathBuilder.AddCubicBezier(new Vector2(458.172f, 67.2079f), new Vector2(463.369f, 65.5213f), new Vector2(465.369f, 61.6826f));
        pathBuilder.AddCubicBezier(new Vector2(468.64f, 56.0306f), new Vector2(468.719f, 49.2813f), new Vector2(468.771f, 42.88f));
        pathBuilder.AddCubicBezier(new Vector2(468.64f, 36.6826f), new Vector2(468.391f, 30.0787f), new Vector2(464.969f, 24.6826f));
        pathBuilder.AddCubicBezier(new Vector2(462.937f, 21.4013f), new Vector2(458.568f, 20.0787f), new Vector2(455.031f, 21.5f));
        pathBuilder.EndFigure(CanvasFigureLoop.Open);

        return new(pathBuilder, 475.1199951171875f, 87.598602294921875f);
    }
}