# ToDoList Web Application

## Описание
Это веб-приложение для управления списком задач (ToDolist), разработанное с использованием ASP.NET Core MVC. Приложение позволяет пользователям регистрироваться, входить в систему и управлять своими задачами.

## Функциональность
- Регистрация и аутентификация пользователей
- Добавление, просмотр, редактирование и удаление задач
- Фильтрация задач по статусу и приоритету
- Web API для управления задачами и учетными записями

## Технологии
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Bootstrap

## Требования
- .NET SDK 6.0
- SQL Server
- SQL Server Management Studio

## Установка локальной базы данных
1. Файл `ToDoListDb.bak` переместить в любую созданную папку на диске `C:\`, например, `C:\DataBaseBackups\ToDoListDb.bak`
1. Открыть SQL SSMS
2. В обозревателе объектов ПКМ нажать на `Базы данных`
3. В контекстном меню выбрать `Восстановить базу данных...`
4. В окне `Восстановление базы данных` выбрать пункт `Устройсто` и нажать на `...`
5. В окне `Выбор устройства резервного копирования` выбрать тип носителя резервной копии `Файл` и нажать на кнопку `Добавить`
6. В проводнике найти и выбрать файл `ToDoListDb.bak`
7. Нажать на кнопку `Ок`
8. В окне `Выбор устройства резервного копирования` выбрать добавленный путь носителя резервной копии и нажать `Ок`
9. После загрузки резервной копии в окне `Восстановление базы данных` нажать `Ок`
10. База данных успешно добавлена

## Настройка подключения базы данных в проекте
1. Открыть файл `ToDoList.sln`
2. В проекте открыть файл `appsettings.json`
3. Если сервер не требует аутентификации, строка подключения:
```
Server = localhost; Database = ToDoListDb; Trusted_Connection = True; TrustServerCertificate=True;
```
5. Если сервер требует аутентификацию, строка подключения:
```
Server = localhost; Database = ToDoListDb; User ID = your_username; Password = your_password; Trusted_Connection=False; TrustServerCertificate=True;
```

## Запуск проекта
1. Открыть файл `ToDoList.sln`
2. Нажать на кнопку `Запуск без отладки` или сочетание клавиш `Ctrl+F5`

## Web API
- User Registration<br>
URL: `POST /api/account/register`<br>
Body:
```json
{
  "Name": "Имя пользователя",
  "Email": "user@example.com",
  "Password": "пароль"
}
```
- User Login<br>
URL: `POST /api/account/login`<br>
Body:
```json
{
  "Email": "user@example.com",
  "Password": "пароль"
}
```
- Get All Tasks<br>
URL: `GET /api/todo`<br>

- Get Task by ID<br>
URL: `GET /api/todo/{id}`<br>

- Add Task<br>
URL: `POST /api/todo`<br>
Body:
```json
{
  "Title": "Название задачи",
  "Description": "Описание задачи",
  "IsCompleted": "Невыполнена",
  "DueDate": "2024-12-31",
  "Priority": 1
}
```
- Edit Task<br>
URL: `PUT /api/todo/{id}`<br>
Body:
```json
{
  "Id": 1,
  "Title": "Новое название задачи",
  "Description": "Новое описание задачи",
  "IsCompleted": "Выполнена",
  "DueDate": "2024-12-31",
  "Priority": 2
}
```
- Delete Task<br>
URL: `DELETE /api/todo/{id}`<br>
