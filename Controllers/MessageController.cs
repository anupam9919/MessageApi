// First, update your MessageController with Swagger documentation attributes

using Microsoft.AspNetCore.Mvc;
using MessageAPI.Authentication;
using MessageApi.Services;
using MessageApi.Models;
using System.Collections.Generic;
using System;

namespace MessageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    [Produces("application/json")]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _MessageService;
        public MessageController(MessageService MessageService)
        {
            _MessageService = MessageService;
        }

        /// <summary>
        /// Inserts a new WhatsApp message into the database
        /// </summary>
        /// <param name="message">Message object containing all required details</param>
        /// <returns>The ID of the newly created message</returns>
        /// <response code="200">Returns the new message ID and success message</response>
        /// <response code="500">If there was an error processing the request</response>
        [HttpPost]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(object), 500)]
        public IActionResult InsertMessage([FromBody] Message message)
        {
            try
            {
                int newMsgId = _MessageService.InsertMessage(message);
                return Ok(new { MsgId = newMsgId, Message = "The message inserted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while inserting the message", Error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves WhatsApp messages based on user mobile, project, and location
        /// </summary>
        /// <param name="userMob">User's mobile number</param>
        /// <param name="project">Project name</param>
        /// <param name="location">Location name</param>
        /// <returns>List of messages with text, date, and status</returns>
        /// <response code="200">Returns the matching messages</response>
        /// <response code="400">If any required parameter is missing</response>
        /// <response code="404">If no messages are found</response>
        /// <response code="500">If there was an error processing the request</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<MessageResponse>), 200)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(typeof(object), 500)]
        public IActionResult GetMessages([FromQuery] string userMob, [FromQuery] string project, [FromQuery] string location)
        {
            try
            {
                if (string.IsNullOrEmpty(userMob) || string.IsNullOrEmpty(project) || string.IsNullOrEmpty(location))
                {
                    return BadRequest(new { Message = "UserMob, Project, and Location parameters are required" });
                }

                var messages = _MessageService.GetMessages(userMob, project, location);

                if (messages.Count == 0)
                {
                    return NotFound(new { Message = "No messages found with the provided parameters" });
                }

                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving messages", Error = ex.Message });
            }
        }
    }
}