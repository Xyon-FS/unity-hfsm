namespace FsmShowcase.Examples.Player
{
    // Inputs supplied by the caller; outputs consumed by a movement component.
    // No input polling, collision queries or actual movement is performed here.
    public sealed class PlayerContext
    {
        public bool IsGrounded { get; set; } = true;
        public float MoveInput { get; set; }
        public float DeltaTime { get; set; }
        public float MoveSpeed { get; set; } = 5f;
        public float Gravity { get; set; } = -20f;
        public float HorizontalSpeed { get; set; }
        public float VerticalSpeed { get; set; }
    }
}
