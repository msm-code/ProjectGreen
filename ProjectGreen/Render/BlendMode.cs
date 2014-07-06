using OpenTK.Graphics.OpenGL;

namespace ProjectGreen.Render
{
    enum BlendEquation
    {
        Add = BlendEquationMode.FuncAdd,
        Sub = BlendEquationMode.FuncSubtract,
        ReverseSub = BlendEquationMode.FuncReverseSubtract,
        Min = BlendEquationMode.Min,
        Max = BlendEquationMode.Max
    }

    enum BlendSrc
    {
        ConstantAlpha = BlendingFactorSrc.ConstantAlpha,
        ConstantColor = BlendingFactorSrc.ConstantColor,
        DstAlpha = BlendingFactorSrc.DstAlpha,
        DstColor = BlendingFactorSrc.DstColor,
        One = BlendingFactorSrc.One,
        OneMinusConstantAlpha = BlendingFactorSrc.OneMinusConstantAlpha,
        OneMinusConstantColor = BlendingFactorSrc.OneMinusConstantColor,
        OneMinusDstAlpha = BlendingFactorSrc.OneMinusDstAlpha,
        OneMinusDstColor = BlendingFactorSrc.OneMinusDstColor,
        OneMinusSrcAlpha = BlendingFactorSrc.OneMinusSrcAlpha,
        SrcAlpha = BlendingFactorSrc.SrcAlpha,
        SrcAlphaSaturate = BlendingFactorSrc.SrcAlphaSaturate,
        Zero = BlendingFactorSrc.Zero
    }

    enum BlendDst
    {
        ConstantAlpha = BlendingFactorDest.ConstantAlpha,
        ConstantColor = BlendingFactorDest.ConstantColor,
        DstAlpha = BlendingFactorDest.DstAlpha,
        DstColor = BlendingFactorDest.DstColor,
        One = BlendingFactorDest.One,
        OneMinusConstantAlpha = BlendingFactorDest.OneMinusConstantAlpha,
        OneMinusConstantColor = BlendingFactorDest.OneMinusConstantColor,
        OneMinusDstAlpha = BlendingFactorDest.OneMinusDstAlpha,
        OneMinusDstColor = BlendingFactorDest.OneMinusDstColor,
        OneMinusSrcAlpha = BlendingFactorDest.OneMinusSrcAlpha,
        OneMinusSrcColor = BlendingFactorDest.OneMinusSrcColor,
        SrcAlpha = BlendingFactorDest.SrcAlpha,
        SrcColor = BlendingFactorDest.SrcColor,
        Zero = BlendingFactorDest.Zero
    }

    struct BlendMode
    {
        public BlendMode(BlendEquation equation, BlendSrc src, BlendDst dst)
            :this()
        {
            this.Equation = equation;
            this.Source = src;
            this.Destination = dst;
        }

        public BlendEquation Equation { get; set; }
        public BlendSrc Source { get; set; }
        public BlendDst Destination { get; set; }
    }
}
