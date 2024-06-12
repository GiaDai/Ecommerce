graph TD
subgraph OrderStateMachine
CreateOrderMessage --> Thiết lập trạng thái: OrderCreated
Thiết lập trạng thái: OrderCreated --> Gửi sự kiện: OrderCreatedEvent
Gửi sự kiện: OrderCreatedEvent --> Chuyển sang trạng thái: OrderCreated
end
Chuyển sang trạng thái: OrderCreated --> Gửi yêu cầu đặt chỗ hàng trong kho
Gửi yêu cầu đặt chỗ hàng trong kho --> |Đặt chỗ thành công| StockReservedEvent
|Đặt chỗ thất bại| StockReservationFailedEvent
StockReservedEvent --> Cập nhật trạng thái: StockReserved
Cập nhật trạng thái: StockReserved --> Gửi yêu cầu thanh toán
Gửi yêu cầu thanh toán --> |Thanh toán thành công| PaymentCompletedEvent
|Thanh toán thất bại| PaymentFailedEvent
PaymentCompletedEvent --> Cập nhật trạng thái: PaymentCompleted
Cập nhật trạng thái: PaymentCompleted --> Xác nhận đơn hàng
Xác nhận đơn hàng --> Gửi email thông báo cho khách hàng
Gửi email thông báo cho khách hàng --> Hoàn thành đơn hàng
PaymentFailedEvent --> Cập nhật trạng thái: PaymentFailed
Cập nhật trạng thái: PaymentFailed --> Gửi email thông báo lỗi cho khách hàng
Gửi email thông báo lỗi cho khách hàng --> Hủy đơn hàng
StockReservationFailedEvent --> Cập nhật trạng thái: StockReservationFailed
Cập nhật trạng thái: StockReservationFailed --> Gửi email thông báo lỗi cho khách hàng
Gửi email thông báo lỗi cho khách hàng --> Hủy đơn hàng
