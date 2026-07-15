# MediatR sử dụng Dependency Injection + Reflection + Generic Resolution để tìm đúng Handler tương ứng với Request.

Ví dụ của bạn:
    public sealed record LoginCommand(
        string UserNameOrEmail,
        string Password)
        : IRequest<Result<LoginResponse>>;
và: 
    public sealed class LoginCommandHandler
        : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        public async Task<Result<LoginResponse>> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            ...
        }
    }

Khi gọi:
    var result = await _mediator.Send(
        new LoginCommand("admin", "123456"));
MediatR sẽ tìm (trong DI Container.):
    IRequestHandler<LoginCommand, Result<LoginResponse>>

# Điều gì xảy ra khi bạn gọi _mediator.Send(command)?
Khi runtime chạy đến dòng _mediator.Send(command), quy trình "phân phát" (Dispatching) của MediatR diễn ra như sau:
    1.Xác định kiểu dữ liệu (Type Mapping):

        Bước 1.MediatR sẽ kiểm tra object command được truyền vào và xác định chính xác kiểu dữ liệu của nó (ví dụ: CreateProductCommand). Từ đó, nó suy ra kiểu của Handler tương ứng cần tìm là IRequestHandler<CreateProductCommand, ProductDto>.

    2.Yêu cầu DI Container cấp phát:

        Bước 2.IMediator (thực chất là class Mediator triển khai nó) sẽ gọi ngầm vào DI Container của .NET (thông qua IServiceProvider) bằng câu lệnh tương tự như:serviceProvider.GetService<IRequestHandler<CreateProductCommand, ProductDto>>()

    3.Khởi tạo Handler cùng các Dependency khác:

        Bước 3.DI Container của .NET sẽ khởi tạo class CreateProductHandler. Nếu bản thân Handler đó lại cần các service khác (như DbContext hay IEmailService), DI Container cũng sẽ tự động inject các service đó vào constructor của Handler.

    4.Thực thi method Handle:

        Bước 4.Sau khi đã có instance của Handler, MediatR sẽ gọi method Handle(command, cancellationToken) của class đó và trả kết quả ngược lại cho API Endpoint/Controller của bạn.Tóm lại: IMediator không tự mình quản lý việc khởi tạo các class thực thi. Nó đóng vai trò như một người môi giới (Broker). Nó dựa vào kiểu dữ liệu của Request để "hỏi" DI Container của .NET xem "Handler nào chịu trách nhiệm cho Request này?", lấy Handler đó ra rồi kích hoạt method Send.
