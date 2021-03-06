﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Messaging;
using DTO;

namespace CustomerClient
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Danh sách chi tiết hóa đơn để bind vào <see cref="OrderDetailsDataGridView"/>.
        /// </summary>
        public BindingList<OrderDetails> orderDetails = new BindingList<OrderDetails>();

        /// <summary>
        /// Object để thao tác với message queue.
        /// </summary>
        private MessageQueue messageQueue;

        public MainForm()
        {
            InitializeComponent();
            InitQueue();

            /// Bind danh sách chi tiết hóa đơn vào view.
            /// Khi view cập nhật thì danh sách dưới code cũng tự động cập nhật.
            OrderDetailsDataGridView.DataSource = orderDetails;
        }

        /// <summary>
        /// Khởi tạo queue.
        /// </summary>
        private void InitQueue()
        {
            messageQueue = MessageQueue.Exists(Constants.QueuePath) // Kiểm tra xem queue này đã được tạo trước đó chưa.
                ? new MessageQueue(Constants.QueuePath, QueueAccessMode.Send) // Nếu tạo rồi thì liên kết tới nó, chỉ cho gửi.
                : MessageQueue.Create(Constants.QueuePath, true); // Chưa tạo thì tạo cái queue mới.
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
