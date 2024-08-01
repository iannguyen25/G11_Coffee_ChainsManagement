using System.ComponentModel.DataAnnotations;

public class Cafe
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên chi nhánh là bắt buộc")]
    [StringLength(100, ErrorMessage = "Tên chi nhánh không được vượt quá 100 ký tự")]
    public string Name { get; set; }

    [StringLength(200, ErrorMessage = "Địa chỉ không được vượt quá 200 ký tự")]
    public string Address { get; set; }

    [StringLength(10, ErrorMessage = "Số điện thoại không được vượt quá 10 ký tự")]
    public string Phone { get; set; }

    [StringLength(200)]
    public string Image { get; set; }
}