using System;
using System.Linq;
using System.Windows.Forms;
using System.Messaging;
using DTO;

namespace AdminClient
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Object để thao tác với message queue.
        /// </summary>
        private MessageQueue messageQueue;

        public MainForm()
        {
            InitializeComponent();
            InitQueue();
            InitOldMessages();

            /// Lắng nghe event khi có 1 message được gửi lên queue.
            messageQueue.BeginReceive();
            messageQueue.ReceiveCompleted += OnNewMessageReceived;
        }

        /// <summary>
        /// Khởi tạo queue.
        /// </summary>
        private void InitQueue()
        {
            messageQueue = MessageQueue.Exists(Constants.QueuePath)  // Kiểm tra xem queue này đã được tạo trước đó chưa.
                ? new MessageQueue(Constants.QueuePath, QueueAccessMode.Receive)  // Nếu tạo rồi thì liên kết tới nó, chi cho đọc & nhận.
                : MessageQueue.Create(Constants.QueuePath, true); // Chưa tạo thì tạo cái queue mới.
        }

        /// <summary>
        /// Lấy các messages đã được gửi sẵn trên queue khi chương trình chưa được chạy.
        /// </summary>
        private void InitOldMessages()
        {
            foreach(System.Messaging.Message message in messageQueue.GetAllMessages()) // Duyệt qua toàn bộ các message còn trên hệ thống.
                AddOrder(ParseOrderFromMessage(message));
        }

        /// <summary>
        /// Hàm được gọi khi có message mới được gửi lên queue.
        /// </summary>
        private void OnNewMessageReceived(object sender, ReceiveCompletedEventArgs e)
        {
            AddOrder(ParseOrderFromMessage(e.Message));
        }

        /// <summary>
        /// Xử lí hóa đơn sau khi nhận được.
        /// </summary>
        /// <param name="order"></param>
        private void AddOrder(Order order)
        {
            if (OrdersListView.InvokeRequired)
            {
                /// Xử lí lỗi không update được view ngoài main-thread.
                Action<Order> callback = AddOrder;
                this.Invoke(callback, new object[] { order });
            }
            else
            {
                /// Thêm thông tin hóa đơn vào giao diện (cần update thêm)
                var item = new ListViewItem(new string[] 
                {
                    order.CustomerName,
                    string.Join(",", order.OrderDetails.Select(od => od.ToString())),
                    order.IsSolved.ToString()
                });
                OrdersListView.Items.Add(item);

                /// Đợi message tiếp theo.
                messageQueue.BeginReceive();
            }

            /// Hiển thị chi tiết hơn...
            /// Thêm code lưu csdl...
        }

        /// <summary>
        /// Chuyển message nhận được về thành thông tin hóa đơn.
        /// </summary>
        private Order ParseOrderFromMessage(System.Messaging.Message message)
        {
            XmlMessageFormatter formatter = new XmlMessageFormatter(new Type[] { typeof(string), typeof(Order) });
            message.Formatter = formatter;
            return Order.FromXml(message.Body.ToString());
        }
    }
}
