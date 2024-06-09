const axios = require('axios');

// Số lượng yêu cầu
const requestCount = 1000;

// Thông tin người dùng (mã sản phẩm, cookie, v.v.)
const users = [
    { ProductId: '1' },
    // Thêm thông tin của các người dùng khác nếu cần
];

// Địa chỉ URL của API
const apiUrl = 'http://localhost:5220/api/orders';

// Tạo một mảng chứa tất cả các promise yêu cầu
const requests = [];

// Kiểm tra nếu danh sách người dùng rỗng
if (users.length === 0) {
    console.log('Không có người dùng nào.');
    return;
}

// Tạo yêu cầu cho mỗi người dùng
for (const user of users) {
    for (let i = 0; i < requestCount; i++) {
        requests.push(sendRequest(user));
    }
}

// Hàm gửi yêu cầu
async function sendRequest(user) {
    try {
        const response = await axios.post(apiUrl, { ProductId: user.ProductId }, { headers: { 'Content-Type': 'application/json', 'Cookie': generateCookie() } });
        return { user, response };
    } catch (error) {
        return { user, error };
    }
}

// Hàm tạo giá trị cookie ngẫu nhiên
function generateCookie() {
    return Math.random().toString(36).substring(2); // Ví dụ đơn giản, bạn có thể sử dụng cách khác để tạo giá trị cookie
}

// Thực thi tất cả các promise đồng thời và xử lý kết quả
Promise.allSettled(requests)
    .then(results => {
        console.log('Kết thúc tất cả các yêu cầu.');
        results.forEach(result => {
            if (result.status === 'fulfilled') {
                console.log('Yêu cầu thành công cho người dùng:', result.value.user);
                // Xử lý kết quả nếu cần
            } else {
                console.error('Yêu cầu thất bại cho người dùng:', result.reason.user, 'Lỗi:', result.reason.error);
            }
        });
    })
    .catch(error => {
        console.error('Lỗi khi xử lý các yêu cầu:', error);
    });
