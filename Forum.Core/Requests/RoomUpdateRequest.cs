﻿namespace Forum.Core.Requests;

public class RoomUpdateRequest
{
    public string Title { get; set; }

    public string Description { get; set; }

    public int CategoryId { get; set; }

}
