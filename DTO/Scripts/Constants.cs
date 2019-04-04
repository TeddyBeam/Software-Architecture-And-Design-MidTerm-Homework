using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    /// <summary>
    /// Class chứa các hằng số sử dụng chung cho project.
    /// Hằng số để đâu cũng được, để chung vô đây cho gọn.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Đường dẫn tới queue.
        /// Dòng \\Private$ để gửi private queue, bỏ ra để gửi public queue.
        /// </summary>
        public const string QueuePath = @".\Private$\MidTermExerciseQueue";
    }
}
