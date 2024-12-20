﻿namespace EventSourcingMedium.API.DTO
{
    public class PostInformationResponseDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
