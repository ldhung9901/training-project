# Code convention
## :star2:Back-end
---
:white_check_mark: Không được sử dụng magic number trong code.
---
```
data1
data2
foo_2
```
:white_check_mark: Tên class phải đặt theo chuẩn: (Tên Module + tên file).
---
```diff
inventory_receiving_repo
inventory_receivingController
inventory_receiving_part
```
:white_check_mark: Tên biến, method phải đặt theo chuẩn snake-case, mô tả đầy đủ chức năng method theo tiếng anh.
---
```diff
caculate_inventory()
```
:white_check_mark: File code phải đặt theo đúng folder, và chức năng và theo tầng (service, repository, controller, database, partial) và có prefix là tên module.
---
```diff
inventory_receiving_repo
inventory_receivingController
inventory_receiving_part

maintenance_receiving_repo
maintenance_receivingController
maintenance_receiving_part
```
:white_check_mark: Bắt buộc phải có HTTP Method (GET, PUT, DELETE, PATCH, POST)cho mỗi action trong controller.
---
```
[HttpPost]
public IActionResult widget_on_Week([FromBody] JObject json)
{
    var id_factory = json.GetValue("id_factory").ToString();
    var model = repo.caculate_report_ưeek(id_factory);
    return Json(model);
}
```
:white_check_mark: Nếu function có thể viết bất đồng bộ thì bắt buộc phải sử dụng async.
---
:white_check_mark: Khi khai báo entity (Database) thì buộc phải có annotion trên mỗi column.
---
```
[Table("maintenance_system_device_group")]
public class maintenance_system_device_group_db
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    [MaxLength(128)]
    public string id { get; set; }
    
    [MaxLength(500)]
    public string name { get; set; }   
    [MaxLength(500)]
    public string note { get; set; }   
    
    // 1 not delete, 2 deleted
    public int? status_del { get; set; }
    [Column(TypeName="DateTime")]
    public DateTime? create_date { get; set; }
    [MaxLength(128)]
    public string create_by { get; set; }
    [MaxLength(128)]
    public string update_by { get; set; }
    [Column(TypeName="DateTime")]
    public DateTime? update_date { get; set; }

}
```
:white_check_mark: Không được sử dụng type "dynamic" khi không cần thiết, muốn sử dụng thì phải xin phép trước.
---
##  :star2:Front-end
---
:white_check_mark: Không được sử dụng any, nếu sử dụng sẽ bị kiểm điểm.
---
:white_check_mark: Bắt buộc phải khai báo type từ phía back-end theo kiểu snake-case.
---
:white_check_mark: Toàn bộ function, variable bắt buộc đều phải khai báo có type và đặt tên theo camel-case.
---
:white_check_mark: Khai báo type phải cùng folder với trang sử dụng type.
---
:white_check_mark: Không được duplicate type, nếu duplicate sẽ bị kiểm điểm.
---
:white_check_mark: Tên component, popup phải đặt theo chuẩn. (tên trang + Component)(tên trang + popUpAddComponent).
---
```
class inventory_process_transfer_indexComponent
class inventory_process_transfer_popUpAddComponent
class inventory_process_transfer_popUpEditComponent
class inventory_process_transfer_popUpViewComponent
```
:white_check_mark: Trang tạo mới sẽ tối thiểu có 2 file index.component.ts, index.component.html.
---
:white_check_mark: Những tính toán liên quan thời gian thì sử dụng thư viện moment js, hạn chế sử dụng Date().
---
---
:white_check_mark: Data truyền vào popup bắt buộc phải có type.
---
:white_check_mark: Tất cả đều phải viết bằng Tailwind CSS, không được sử dụng attribute style trong file html, nếu muốn sử dụng thì viết riêng ra file scss riêng.
---
##  :Mobile-App
:white_check_mark: Tất cả đều phải đặt tên theo camel-case.
---
:white_check_mark: Đặt đúng chức năng của tên function, .
---
:white_check_mark: Tất cả đều phải khai báo type, model.
---
:white_check_mark: Tách riêng API thành file riêng và để sử dụng lại nhiều nhất có thể.
---
:white_check_mark: Không code chung API với Widget.
---
:white_check_mark: Hạn chế sử dụng type dynamic.
---
