namespace Reflectrix.LaserBeam
{
    public interface IBeamReceiver
    {
        bool Receive(LaserBeamPoint[] beamPoint);
    }
}
