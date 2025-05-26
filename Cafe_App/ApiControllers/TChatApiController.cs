using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe_App.ApiControllers;

[Route("api/chat")]
[ApiController]
public class TChatApiController : ControllerBase
{
    // Списки для хранения ожидающих клиентов (для long polling)
    private static List<TaskCompletionSource<string>> WaitingClients = new List<TaskCompletionSource<string>>();

    // Список сообщений для хранения истории (опционально)
    private static List<string> MessagesHistory = new List<string>();

    /// <summary>
    /// Endpoint для длительного опроса новых сообщений.
    /// Клиент делает GET запрос, который ждёт появления нового сообщения.
    /// </summary>
    [HttpGet("poll")] 
    public async Task<IActionResult> Poll(CancellationToken cancellationToken) 
    { 
        // Создаем TaskCompletionSource, который разрешится при появлении нового сообщения
        var tcs = new TaskCompletionSource<string>();

        // Привязываем отмену запроса - если пользователь отменяет запрос (например, закрывает страницу)
        cancellationToken.Register(() => tcs.TrySetCanceled());

        // Регистрируем новый ожидающий запрос
        lock (WaitingClients)
        {
            WaitingClients.Add(tcs);
        }

        string message;
        try
        {
            // Ждем, пока не появится сообщение или запрос будет отменён
            message = await tcs.Task;
        }
        catch (TaskCanceledException)
        {
            // Если запрос отменяется (например, по таймауту), возвращаем 204 (No Content)
            return NoContent();
        }
        return Ok(message);
    }

    /// <summary>
    /// Endpoint для отправки нового сообщения.
    ///  Клиент посылает POST запрос с текстом сообщения и именем пользователя.
    /// Сервер сохраняет сообщение и уведомляет всех ожидающих клиентов.
    /// </summary>
    [HttpPost("send")] 
    public IActionResult Send([FromForm] string message, [FromForm] string userName) 
    { 
        // Формируем сообщение
        var newMessage = $"{userName}: {message}";
        
        // (Опционально) сохраняем в историю
        lock (MessagesHistory)
        { 
            MessagesHistory.Add(newMessage);
        }

        // Уведомляем все ожидающие long polling запросы
        lock (WaitingClients) 
        { 
            foreach (var waiting in WaitingClients) 
            { 
                waiting.TrySetResult(newMessage);
            } 
            WaitingClients.Clear();
        }
        return Ok();
    }
    
    [HttpGet("last")] 
    public IActionResult Last() 
    { 
        return Ok(MessagesHistory);
    }
}
