using ChatApp.Users.Application.Users;
using Grpc.Core;
using MediatR;
using UsersService.Grpc;

namespace ChatApp.Users.Presentation.GrpcServices;

public class UsersGrpcService : UsersService.Grpc.UsersService.UsersServiceBase
{
    private readonly ISender _sender;

    public UsersGrpcService(ISender sender)
    {
        _sender = sender;
    }

    public override async Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request, ServerCallContext context)
    {
        var inputDto = new GetUserByIdInput(Guid.Parse(request.UserId));

        var response = await _sender.Send(inputDto);

        return new GetUserByIdResponse
        {
            UserId = response.UserId.ToString(),
            Email = response.Email,
            Username = response.Username,
            Status = response.Status,
            ProfilePicture = response.ProfilePicture,
        };
    }
}