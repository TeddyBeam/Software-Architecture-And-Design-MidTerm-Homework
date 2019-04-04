using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace DTO
{
    /// <summary>
    /// Hóa đơn đặt hàng.
    /// </summary>
    [Serializable]
    public class Order
    {
        /// <summary>
        /// Tên khách hàng.
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Chi tiết hóa đơn.
        /// </summary>
        public List<OrderDetails> OrderDetails { get; set; }
            
        /// <summary>
        /// Hóa đơn đã được xử lí (bởi Admin) hay chưa?
        /// </summary>
        public bool IsSolved { get; set; }

        /// <summary>
        /// Chuyển hóa đơn thành 1 đoạn string theo chuẩn XML để gửi lên queue.
        /// </summary>
        public string ToXml()
        {
            var serializer = new XmlSerializer(typeof(Order));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, this);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Chuyển 1 đoạn string chuẩn XML lấy từ trên queue lại thành hóa đơn.
        /// </summary>
        /// <param name="xml">Đoạn string theo chuẩn XML</param>
        public static Order FromXml(string xml)
        {
            var serializer = new XmlSerializer(typeof(Order));
            using (var reader = new StringReader(xml))
                return (Order)serializer.Deserialize(reader);
        }
    }
}
