using System.Text.Json;

namespace Domain.Contracts;

public static class DefaultContractTerms
{
    public const int DurationInMonths = 6;

    public const double CommissionRate = 0.2;

    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    private static readonly IReadOnlyList<ContractClause> Clauses =
    [
        new()
        {
            Title = "Điều 1 - Phạm vi ký gửi",
            Content =
                "Bên ký gửi đồng ý ủy quyền cho Bên nhận ký gửi thực hiện việc tiếp nhận, quảng bá, giới thiệu và môi giới cho thuê bất động sản thuộc quyền sở hữu hợp pháp của Bên ký gửi trong thời hạn hợp đồng."
        },

        new()
        {
            Title = "Điều 2 - Cam kết pháp lý tài sản",
            Content =
                "Bên ký gửi cam kết bất động sản thuộc quyền sở hữu hoặc quyền quản lý hợp pháp, không tranh chấp, không kê biên hoặc hạn chế giao dịch."
        },

        new()
        {
            Title = "Điều 3 - Khoản tiền đảm bảo ký gửi",
            Content =
                "Ngay khi ký kết hợp đồng, Bên ký gửi nộp khoản tiền đảm bảo cố định 1.000.000 VNĐ."
        },

        new()
        {
            Title = "Điều 4 - Hình thức nộp tiền đảm bảo",
            Content =
                "Khoản tiền đảm bảo có thể thanh toán bằng tiền mặt hoặc chuyển khoản và được ghi nhận trên hệ thống."
        },

        new()
        {
            Title = "Điều 5 - Thời hạn hợp đồng",
            Content =
                "Thời hạn hiệu lực mặc định là 06 tháng kể từ ngày hợp đồng được phê duyệt trên hệ thống."
        },

        new()
        {
            Title = "Điều 6 - Điều kiện chấm dứt hợp đồng",
            Content =
                "Hợp đồng chấm dứt khi hết hạn, hai bên thỏa thuận chấm dứt hoặc có vi phạm nghiêm trọng."
        },

        new()
        {
            Title = "Điều 7 - Hạn chế hủy ngang trước hạn",
            Content =
                "Bên ký gửi không được đơn phương hủy hợp đồng trong thời hạn 06 tháng nếu bên nhận ký gửi thực hiện đúng nghĩa vụ."
        },

        new()
        {
            Title = "Điều 8 - Hoàn trả tiền đảm bảo",
            Content =
                "Nếu hết 06 tháng không phát sinh giao dịch thuê, bên ký gửi có quyền yêu cầu hoàn tiền trong vòng 07 ngày làm việc."
        },

        new()
        {
            Title = "Điều 9 - Cấn trừ tiền đảm bảo",
            Content =
                "Nếu giao dịch thuê thành công, khoản đảm bảo được cấn trừ vào phí hoa hồng môi giới."
        },

        new()
        {
            Title = "Điều 10 - Hoa hồng môi giới",
            Content =
                "Bên ký gửi thanh toán phí môi giới bằng 5% giá trị hợp đồng thuê thực tế được ký kết thành công giữa Bên ký gửi và khách thuê do Bên nhận ký gửi giới thiệu."
        },

        new()
        {
            Title = "Điều 11 - Nghĩa vụ phối hợp xem nhà",
            Content =
                "Bên ký gửi tạo điều kiện để khảo sát và dẫn khách xem theo lịch hẹn."
        },

        new()
        {
            Title = "Điều 12 - Cung cấp thông tin trung thực",
            Content =
                "Mọi thông tin, hình ảnh, giấy tờ cung cấp phải đầy đủ, chính xác, hợp pháp."
        },

        new()
        {
            Title = "Điều 13 - Quyền quảng bá tài sản",
            Content =
                "Bên nhận ký gửi được quyền đăng tải thông tin tài sản trên hệ thống và các kênh truyền thông."
        },

        new()
        {
            Title = "Điều 14 - Quyền từ chối khách thuê",
            Content =
                "Bên ký gửi có quyền từ chối khách thuê không đáp ứng tiêu chí đã xác lập."
        },

        new()
        {
            Title = "Điều 15 - Điều khoản sửa đổi đặc biệt",
            Content =
                "Mọi điều khoản ngoài bộ chuẩn phải được bộ phận pháp lý rà soát và quản lý phê duyệt."
        },

        new()
        {
            Title = "Điều 16 - Hiệu lực hợp đồng",
            Content =
                "Hợp đồng có hiệu lực kể từ thời điểm được xác nhận phê duyệt trên hệ thống."
        }
    ];

    public static string Serialize()
    {
        return JsonSerializer.Serialize(Clauses, JsonOptions);
    }

    public static string Serialize(IEnumerable<ContractClause> contractClauses)
    {
        return JsonSerializer.Serialize(
            contractClauses,
            JsonOptions);
    }

    public static IReadOnlyList<ContractClause> Deserialize(string json)
    {
        return JsonSerializer.Deserialize<List<ContractClause>>(json, JsonOptions)
               ?? [];
    }
}
