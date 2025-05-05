namespace Reflectrix.LaserBeam
{
    public interface IBeamReceiver
    {
        bool Receive(ILaserBeamPoint[] beamPoint);
    }
}
