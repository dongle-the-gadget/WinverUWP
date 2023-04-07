#pragma once

namespace WinverUWP::Native
{
    public ref class VectorDetours sealed
    {
    private:
        VectorDetours() {}

    public:
        static Platform::Boolean EnableVectorRendering();
    };
}
