# Mục đích thật sự

    Refresh Token đại diện cho:
        "Phiên đăng nhập"
        chứ không phải
        "Token để lấy JWT"
        Nó chính là Session.

    Trong thiết kế ecommerce-microservices, Refresh Token được sử dụng để giải quyết bài toán cân bằng giữa bảo mật và trải nghiệm người dùng khi triển khai cơ chế xác thực dựa trên JWT (JSON Web Token).

# Identity Service nên triển khai các nghiệp vụ sau:

    Login → tạo Access Token + Refresh Token.
    Refresh Token → kiểm tra token còn hiệu lực, rotate sang Refresh Token mới và thu hồi token cũ.
    Logout → thu hồi Refresh Token hiện tại.
    Logout All Devices → thu hồi toàn bộ Refresh Token của người dùng.
    Change Password → thu hồi toàn bộ Refresh Token để buộc đăng nhập lại trên mọi thiết bị.
    Reuse Detection → phát hiện việc sử dụng lại Refresh Token đã bị thu hồi (dấu hiệu token bị đánh cắp), từ đó có thể vô hiệu hóa toàn bộ các phiên của người dùng và yêu cầu xác thực lại.

    Đây là cách triển khai phổ biến trong các hệ thống microservices quy mô lớn vì vừa giữ được ưu điểm stateless của JWT cho các service nghiệp vụ, vừa đảm bảo khả năng quản lý phiên đăng nhập và thu hồi quyền truy cập khi cần thiết.

1. Vấn đề khi chỉ sử dụng Access Token

    Thông thường sau khi đăng nhập:

        User Login
            |
            v
        Authentication Service
            |
            +--> Access Token (15 phút)

    Access Token thường có thời gian sống ngắn:
        {
        "sub": "user123",
        "role": "customer",
        "exp": 1720864200
        }


    Nếu chỉ dùng Access Token:

        Token hết hạn sau 15 phút.
        Người dùng phải login lại.
        Trải nghiệm mua sắm bị gián đoạn.

        Ví dụ:
        Khách hàng:
        - Thêm sản phẩm vào giỏ hàng
        - Xem chi tiết sản phẩm
        - Chờ 20 phút
        - Thanh toán

        => Access Token hết hạn
        => Bị đá về màn hình Login

    Đây là trải nghiệm không tốt cho ecommerce.

2. Mục đích của Refresh Token
    Refresh Token cho phép hệ thống cấp lại Access Token mới mà không yêu cầu người dùng đăng nhập lại.

    Sau login:
        Authentication Service
            |
            +--> Access Token (15 phút)
            |
            +--> Refresh Token (7 ngày)

    Luồng hoạt động:

        User Request
            |
            +--> Access Token hết hạn
                    |
                    v
            Auth Service
                    |
            Verify Refresh Token
                    |
                    v
            Cấp Access Token mới

    Người dùng không nhận ra quá trình này.

3. Trong Ecommerce-Microservices Refresh Token được đặt ở đâu?

    Thông thường:
        +--------------------+
        | API Gateway        |
        +--------------------+
                |
                v
        +--------------------+
        | Auth Service       |
        +--------------------+
                |
                +--> Redis
                +--> Database
    Auth Service

        Chịu trách nhiệm:
            Login
            Logout
            Refresh Token
            Revoke Token
            Các service khác

        Không xử lý Refresh Token:
            Product Service
            Order Service
            Cart Service
            Inventory Service
            Payment Service

        Chúng chỉ validate Access Token.

4. Tại sao Refresh Token quan trọng trong Microservices?
    4.1 Giảm thời gian sống của Access Token

        Có thể đặt:
            Access Token = 15 phút

            Refresh Token = 7 ngày
            `
        Nếu Access Token bị đánh cắp:
            Thời gian khai thác chỉ 15 phút
            => Giảm rủi ro bảo mật.

    4.2 Hỗ trợ Single Sign-On
        Ví dụ:
            Web App
            Mobile App
            Admin Portal

        Đều dùng chung Auth Service.

        Refresh Token giúp người dùng không phải đăng nhập lại liên tục.
    4.3 Hỗ trợ Logout

        Nếu dùng JWT thuần:
            JWT đã phát hành
            => Khó thu hồi ngay
            ``
        Refresh Token được lưu trong:
            Redis
            Database
        Khi logout:
            DELETE refresh_token

        Sau khi Access Token hết hạn:
            Không thể refresh nữa
            => User bị logout hoàn toàn
    4.4 Kiểm soát phiên đăng nhập

        Có thể lưu:
            UserId
            Device
            IP
            Browser
            CreatedAt
            ExpiredAt
        Ví dụ:
            iPhone 15
            Chrome Windows
            Macbook Safari
        Người dùng có thể:
            "Đăng xuất khỏi tất cả thiết bị"
5. Luồng xác thực đầy đủ
    Login
        Client
        |
        +--> username/password
        |
        v
        Auth Service
        |
        +--> Access Token (15m)
        +--> Refresh Token (7d)

    Gọi API
        Client
            |
            +--> Access Token
            |
            v
        API Gateway
            |
            v
        Product Service

    Access Token hết hạn
        401 Unauthorized

    Client tự động:
        POST /auth/refresh
    Kèm:
        Refresh Token

    Refresh thành công
        {
        "accessToken":"new-token"
        }

    Client tiếp tục gọi API.

6. Best Practice cho Ecommerce Microservices
    Access Token ngắn
        5 - 15 phút
    Refresh Token dài
        7 - 30 ngày
    Lưu Refresh Token trong Redis
        refresh:userId:deviceId
    
    Redis giúp:

        Tra cứu nhanh
        Revoke nhanh
        Scale tốt
        ✅ Rotate Refresh Token

        Mỗi lần refresh:
            RefreshToken_1
                |
                v
            RefreshToken_2

        Token cũ bị vô hiệu.

        Điều này giúp chống:
            Token Replay Attack

    Ví dụ kiến trúc thực tế
                        +-------------+
                        |   Client    |
                        +------+------+ 
                                |
                                v
                        +--------+--------+
                        |   API Gateway   |
                        +--------+--------+
                                |
                        +---------+---------+
                        | Auth Service      |
                        +---------+---------+
                                |
                        Redis / PostgreSQL
                                |
        ------------------------------------------------
        | Product | Cart | Order | Payment | Inventory |
        ------------------------------------------------


[ Browser (ReactJS) ]                     [ API Gateway (YARP) ]               [ Identity Service ]
       │                                            │                                     │
       │ 1. POST /api/auth/login ──────────────────>│ ───────────────────────────────────>│
       │                                            │                                     │ (Xác thực thành công)
       │ <─ 2. Trả về:                              │ <───────────────────────────────────│ (Tạo cặp Token mới)
       │      - Body JSON: { accessToken }          │ (YARP Forward Headers)              │ (Set HttpOnly Cookie)
       │      - Cookie: refreshToken (HttpOnly) ────│─────────────────────────────────────│
       │                                            │                                     │
       ├────────────────────────────────────────────┼─────────────────────────────────────┤
       │                                            │                                     │
       │ 3. Request API (VD: /api/orders)           │                                     │
       │    Headers: Authorization: Bearer <Access> │                                     │
       │ ──────────────────────────────────────────>│ ──────────────────> [ Order Service ]
       │                                            │                     (Tự verify JWT)
       ├────────────────────────────────────────────┼─────────────────────────────────────┤
       │                                            │                                     │
       │ 4. (Access Token hết hạn - 401 hoặc chủ động)                                   │
       │    POST /api/auth/refresh (Body rỗng) ────>│ ───────────────────────────────────>│
       │    Browser tự đính kèm HttpOnly Cookie     │                                     │ (Verify Refresh Token)
       │                                            │                                     │ (Xoay vòng token)
       │ <─ 5. Trả về Access Token mới (JSON Body)  │ <───────────────────────────────────│ (Set Cookie Refresh mới)
       │      & ghi đè Cookie Refresh mới           │

# Bản chất HttpOnly Cookie là gì?
    Không thể chạm tới bằng Code (HttpOnly): Khi trình duyệt nhận được một Cookie có flag HttpOnly, nó sẽ khóa chặt không cho bất kỳ đoạn mã JavaScript nào (document.cookie) truy cập được. Hacker có chèn được script phá hoại cũng không cách nào đọc được refreshToken.

    Tự động gửi đính kèm: Khi gọi API /api/auth/refresh, trình duyệt sẽ tự động đính kèm Cookie này vào Header của request mà anh không cần viết một dòng code JS nào để gán nó vào.

    Chống giả mạo yêu cầu chéo (CSRF): Để an toàn tuyệt đối khi dùng Cookie, ta cần kết hợp thêm flag SameSite=Strict hoặc Lax và Secure (chỉ truyền qua HTTPS).