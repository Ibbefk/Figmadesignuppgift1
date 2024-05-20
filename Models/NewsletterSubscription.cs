namespace Figmadesign.Models
{
    public class NewsletterSubscription
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty; 
        public bool DailyNewsletter { get; set; }
        public bool AdvertisingUpdates { get; set; }
        public bool WeekInReviews { get; set; }
        public bool EventUpdates { get; set; }
        public bool StartupWeekly { get; set; }
        public bool Podcasts { get; set; }
    }
}
