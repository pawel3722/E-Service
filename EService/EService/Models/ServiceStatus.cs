namespace EService.Models
{
    public static class ServiceStatus
    {
        public const int Created = 0;           // 0 -> Utworzono
        public const int AssignedWorker = 1;    // 1 -> Przypisano pracownika
        public const int WaitingForAPart = 2;   // 2 -> (awaryjnie) Oczekiwanie na część
        public const int Finished = 3;          // 3 -> Ukończono
    }
}
