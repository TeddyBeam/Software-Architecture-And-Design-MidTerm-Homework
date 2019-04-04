using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Messaging;
using DTO;

namespace CustomerClient
{
    public partial class MainForm : Form
    {
        public BindingList<OrderDetails> orderDetails = new BindingList<OrderDetails>();
        private MessageQueue messageQueue;

        public MainForm()
        {
            InitializeComponent();
            InitQueue();
            OrderDetailsDataGridView.DataSource = orderDetails;
        }

        /// <summary>
        /// Khởi tạo queue.
        /// </summary>
        private void InitQueue()
        {
            messageQueue = MessageQueue.Exists(Constants.QueuePath)
                ? new MessageQueue(Constants.QueuePath, QueueAccessMode.Send)
                : MessageQueue.Create(Constants.QueuePath, true);
        }

        /// <summary>
        /// Gửi hóa đơn lên queue.
        /// </summary>
        /// <param name="order">Hóa đơn muốn gửi.</param>
        public void SendOrder(Order order)
        {
            var message = order.ToXml();
            MessageQueueTransaction transaction = new MessageQueueTransaction();
            transaction.Begin();
            messageQueue.Send(message, transaction);
            transaction.Commit();
            MessageBox.Show("Gửi hóa đơn thành công, dữ liệu:\n" + message);
        }

        /// <summary>
        /// Hàm được gọi khi nhấn nút "Gửi".
        /// </summary>
        private void SendBtn_Click(object sender, EventArgs e)
        {
            SendOrder(new Order()
            {
                CustomerName = CustomerNameTextBox.Text,
                OrderDetails = orderDetails.ToList()
            });
        }
    }
}
