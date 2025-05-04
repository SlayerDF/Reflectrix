namespace Reflectrix.LaserBeam
{
    public interface IBeamReceiverAndEmitter
    {
        LaserBeamPoint[] ReceiveAndEmit(LaserBeamPoint[] beamPoints);
    }
}
