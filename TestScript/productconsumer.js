const amqp = require('amqplib');

async function startConsumer() {
    try {
        // Tạo kết nối tới RabbitMQ server
        const connection = await amqp.connect({
            hostname: 'localhost',
            port: 5672,
            vhost: '/',
            username: 'ecommerce',
            password: '123456789',
        }); // Adjust the connection string as needed
        const channel = await connection.createChannel();

        // Đảm bảo hàng đợi tồn tại
        const queue = 'rpc_queue';
        await channel.assertQueue(queue, { durable: true });

        console.log(`[*] Đang chờ message trong hàng đợi ${queue}. Nhấn CTRL+C để thoát`);

        // Thiết lập tiêu dùng
        channel.consume(queue, async (msg) => {
            if (msg !== null) {
                const content = msg.content.toString();
                const receivedMessage = JSON.parse(content);
                const placeOrderRequest = receivedMessage.message;

                console.log(`[x] Đã nhận message: ${content}`);

                // Xử lý message và tạo phản hồi
                const response = {
                    message: `Product nodejs consumer for Product Id ${placeOrderRequest.productId} placed on ${placeOrderRequest.orderDate}`
                };
                console.log(`[x] Đã xử lý message: ${JSON.stringify(response)}`);
                try {
                    // Gửi phản hồi lại tới hàng đợi replyTo
                    const sendResult = channel.sendToQueue(msg.properties.replyTo, Buffer.from(JSON.stringify(response)), {
                        correlationId: msg.properties.correlationId,
                        headers: msg.properties.headers // Thêm headers để đảm bảo MassTransit nhận đúng phản hồi
                    });

                    if (sendResult) {
                        console.log('[x] Gửi phản hồi thành công');
                    } else {
                        console.error('[!] Gửi phản hồi thất bại');
                    }

                    // Xác nhận message
                    channel.ack(msg);
                } catch (error) {
                    console.error(`[!] Lỗi khi gửi phản hồi: ${error.message}`);
                    // Bạn có thể quyết định không xác nhận message ở đây để nó có thể được xử lý lại
                }
            }
        });
    } catch (error) {
        console.error(`[!] Lỗi: ${error.message}`);
    }
}

startConsumer();
