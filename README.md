DotNet API Auth Crud
===
Abstract:xxx
## Papar Information
- Title:  `paper name`
- Authors:  `A`,`B`,`C`
- Preprint: [https://arxiv.org/abs/xx]()
- Full-preprint: [paper position]()
- Video: [video position]()

## Install & Dependence
- python
- pytorch
- numpy

## Dataset Preparation
| Dataset | Download |
| ---     | ---   |
| dataset-A | [download]() |
| dataset-B | [download]() |
| dataset-C | [download]() |

## Use
- for train
  ```
  python train.py
  ```
- for test
  ```
  python test.py
  ```

## API Endpoints

| Method	| Endpoint	| Deskripsi | 
| ---     | ---   | ---   |
| POST	| /api/auth/register	| Mendaftarkan user baru | 
| POST	| /api/auth/login	| Login dan mendapatkan token JWT | 
| GET	| /api/users	| Mendapatkan semua user (butuh token) | 
| GET	| /api/users/{id}	| Mendapatkan user berdasarkan ID (butuh token) | 
| PUT	| /api/users/{id}	| Mengupdate user (butuh token)
| DELETE	| /api/users/{id}	| Menghapus user (butuh token) | 

## Pretrained model
| Model | Download |
| ---     | ---   |
| Model-1 | [download]() |
| Model-2 | [download]() |
| Model-3 | [download]() |


## Directory Hierarchy
```
|—— Controllers
|    |—— AuthController.cs
|—— Data
|    |—— AppDbContext.cs
|—— Migrations
|    |—— 20250321031412_InitialCreate.Designer.cs
|    |—— 20250321031412_InitialCreate.cs
|    |—— AppDbContextModelSnapshot.cs
|—— Models
|    |—— Product.cs
|    |—— User.cs
|—— MyAuthCrudApi.csproj
|—— MyAuthCrudApi.http
|—— Program.cs
|—— Properties
|    |—— launchSettings.json
|—— Repositories
|    |—— IUserRepository.cs
|    |—— UserRepository.cs
|—— Services
|    |—— AuthService.cs
|—— appsettings.Development.json
|—— appsettings.json
|—— 
```
## Code Details
### Tested Platform
- software
  ```
  OS: Debian unstable (May 2021), Ubuntu LTS
  Python: 3.8.5 (anaconda)
  PyTorch: 1.7.1, 1.8.1
  ```
- hardware
  ```
  CPU: Intel Xeon 6226R
  GPU: Nvidia RTX3090 (24GB)
  ```
### Hyper parameters
```
```
## References
- [paper-1]()
- [paper-2]()
- [code-1](https://github.com)
- [code-2](https://github.com)
  
## License

## Citing
If you use xxx,please use the following BibTeX entry.
```
```
