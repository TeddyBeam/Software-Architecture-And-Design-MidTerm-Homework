using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    /// <summary>
    /// Chi tiết hóa đơn.
    /// </summary>
    [Serializable]
    public class OrderDetails
    {
        /// <summary>
        /// Tên mặt hàng.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Số lượng đặt mua.
        /// </summary>
        public int Amount { get; set; }

        public override string ToString()
        {
            return string.Format("[Tên: {0}, Số lượng: {1}]", ProductName, Amount);
        }
    }
}
