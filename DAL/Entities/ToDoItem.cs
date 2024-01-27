﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DAL.Entities
{
    [DataContract]
    [Table("ToDoItems")]
    public class ToDoItem
    {
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public required string Title { get; set; }

        public string? Description { get; set; }

        [EnumDataType(typeof(ToDoItemStatusType))]
        public ToDoItemStatusType Status { get; set; }

        public required DateTime CreatedTime { get; set; }

        public DateTime? CompletionTime { get; set; }

        public DateTime? TimeSpent { get; set; }

        [ForeignKey("ParentItemId")]
        public List<ToDoItem>? SubItems { get; set; }

        public Guid? ParentItemId { get; set; }
        
        [Required]
        public required Guid UserId;

        [ForeignKey("UserId")]
        [Required]
        public required User User { get; set; }

    }
}
