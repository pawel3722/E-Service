namespace EService.Models
{
    public static class OrderStatus
    {
        public const int StartedProcessing = 0;// -> Przyjęto do realizacji 
        public const int AssignedManager = 1;  // 1 -> Przypisano menadżera
        public const int FinishedAnalysis = 2; // 2 -> Ukończono ekspertyzę
        public const int AssignedActions = 3;  // 3 -> Zlecono wykonanie działań
        public const int Finished = 4;         // 4 -> Naprawiono
        public const int Received = 5;         // 5 -> Odebrano
    }
}
