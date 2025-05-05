namespace Reflectrix.LaserBeam
{
    public interface IBeamReceiverAndEmitter
    {
        ILaserBeamPoint[] ReceiveAndEmit(ILaserBeamPoint[] beamPoints);
    }
}
