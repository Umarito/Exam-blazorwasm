# AVRANG PROJECT - TESTING CHECKLIST

## ✅ ПОЛНЫЙ CHECKLIST ФУНКЦИОНАЛЬНОСТИ

### 1. ЗАПУСК ПРИЛОЖЕНИЙ

- [ ] WebAPI запущен на http://localhost:5094
  - Проверка: Откройте http://localhost:5094/swagger/index.html
  
- [ ] Admin Panel запущен на http://localhost:5001
  - Проверка: Откройте http://localhost:5001/login

---

### 2. ТЕСТИРОВАНИЕ API (Swagger)

#### Products
- [ ] GET /api/products - Получить список продуктов
  - Результат: Должны быть 8+ продуктов
  
- [ ] GET /api/products/{id} - Получить продукт по ID (например, 1)
  - Результат: iPhone 15 Pro Max с ценой 999
  
- [ ] POST /api/auth/login - Получить JWT токен
  - Email: admin@gmail.com
  - Password: Admin123
  - Результат: Должен вернуть токен

#### Authentication (с токеном)
- [ ] Скопируйте полученный токен из login ответа
- [ ] Нажмите кнопку "Authorize" в Swagger
- [ ] Вставьте: `Bearer {token}` в поле
- [ ] Теперь все protected endpoints должны быть доступны

#### Categories
- [ ] GET /api/categories - Получить категории
- [ ] POST /api/categories - Создать новую категорию (с токеном)
  - Name: "Electronics"
  - Slug: "electronics"
  - Description: "Electronic devices"

#### Brands
- [ ] GET /api/brands - Получить бренды
- [ ] POST /api/brands - Создать бренд (с токеном)
  - Name: "Sony"
  - Slug: "sony"

#### Installments
- [ ] GET /api/installments - Получить планы рассрочки
  - Результат: 4 плана (3, 6, 12, 24 месяца)

---

### 3. ТЕСТИРОВАНИЕ ADMIN PANEL

#### Login
- [ ] Откройте http://localhost:5001/login
- [ ] Нажмите "Register here" - перенаправляет на /register
- [ ] Вернитесь на login
- [ ] Введите: admin@gmail.com / Admin123
- [ ] Нажмите Login
- [ ] ✅ Должны быть перенаправлены на Dashboard (/)

#### Dashboard
- [ ] Видите Dashboard с статистикой
- [ ] Должны видеть карточки:
  - Products count
  - Categories count
  - Brands count  
  - Installments count
- [ ] Каждая карточка должна иметь эффект hover (поднимается и тень)

#### Navigation Menu
- [ ] Видите меню с пунктами:
  - Dashboard 📦
  - Products 📦
  - Categories 📂
  - Brands 🏷️
  - Installments 💳
  - Logout
- [ ] Нажмите на каждый пункт - должна открыться соответствующая страница

#### Products Management
- [ ] Откройте Products из меню
- [ ] Видите таблицу с продуктами (минимум 8 продуктов)
- [ ] Каждый продукт имеет:
  - ID, изображение, Name, Price, Category, Actions (Edit/Delete)
- [ ] Нажмите "Add New Product"
  - Открывается modal диалог
  - Заполните форму:
    - Name: "Test Product"
    - Description: "Test"
    - Price: 100
    - Image URL: (любой URL)
    - Category: (выберите любую)
    - Featured: (галочка)
  - Нажмите "Save Changes"
  - ✅ Должен появиться в таблице
- [ ] Нажмите "Edit" для одного продукта
  - Заполните данные (например, измените цену)
  - Нажмите "Save Changes"
  - ✅ Данные должны обновиться в таблице
- [ ] Нажмите "Delete" для одного продукта
  - ✅ Продукт должен удалиться из таблицы

#### Categories Management
- [ ] Откройте Categories из меню
- [ ] Видите таблицу с категориями
- [ ] Нажмите "Add Category"
  - Открывается modal
  - Заполните:
    - Name: "Sports"
    - Slug: "sports"
    - Description: "Sports equipment"
  - Нажмите "Save"
  - ✅ Категория должна появиться в таблице
- [ ] Нажмите "Edit" для категории
  - Измените данные
  - Нажмите "Save"
  - ✅ Данные обновиться

#### Brands Management
- [ ] Откройте Brands из меню
- [ ] Видите карточки с брендами
- [ ] Нажмите "Add Brand"
  - Заполните:
    - Brand Name: "Nike"
    - Slug: "nike"
  - Нажмите "Add Brand"
  - ✅ Бренд должен появиться на странице
- [ ] Нажмите "Delete" для бренда
  - ✅ Бренд должен удалиться

#### Installments Management
- [ ] Откройте Installments из меню
- [ ] Видите 4 карточки с планами
- [ ] Для каждого видите: месяцы и процент
- [ ] Нажмите "Add Plan"
  - Месяцы: 36
  - Процент: 6.5
  - Нажмите "Add Plan"
  - ✅ Новый план должен появиться
- [ ] Нажмите "Delete" для одного плана
  - ✅ План должен удалиться

#### UI/UX Effects
- [ ] Наведите мышвку на кнопки - должны иметь hover эффект
- [ ] Наведите на карточку - должна подняться и тень увеличится
- [ ] Наведите на строку таблицы - фон изменится
- [ ] Все переходы плавные и красивые

#### Logout
- [ ] Найдите кнопку Logout в меню
- [ ] Нажмите на неё
- [ ] ✅ Должны быть перенаправлены на login страницу
- [ ] Браузер должен очистить токен
- [ ] При попытке открыть /products - перенаправит на login

---

### 4. ПАРАМЕТРЫ URL И НАВИГАЦИЯ

- [ ] Откройте Products через URL: http://localhost:5001/products
- [ ] Откройте Categories: http://localhost:5001/categories
- [ ] Откройте Brands: http://localhost:5001/brands
- [ ] Откройте Installments: http://localhost:5001/installments
- [ ] Откройте Dashboard: http://localhost:5001/
- [ ] Откройте Login: http://localhost:5001/login
- [ ] Откройте Register: http://localhost:5001/register

---

### 5. ФОРМА РЕГИСТРАЦИИ

- [ ] Откройте http://localhost:5001/register
- [ ] Видите форму с полями:
  - Full Name
  - Email
  - Password (минимум 8 символов)
  - Confirm Password
- [ ] Заполните форму с новыми данными
- [ ] Пароли не совпадают - должен быть error
- [ ] Заполните правильно и нажмите Register
- [ ] ✅ Должны быть перенаправлены на Dashboard
- [ ] Разлогиньтесь
- [ ] Залогиньтесь с новыми учетными данными
- [ ] ✅ Должны вы успешно войти

---

### 6. API AUTHENTICATION

- [ ] Попробуйте POST /api/products без токена
  - ✅ Должен вернуть 401 Unauthorized
- [ ] Повторите с валидным токеном
  - ✅ Должен вернуть 200 Ok

---

### 7. ВАЛИДАЦИЯ ФОРМ

- [ ] На форме Products нажмите Save без заполнения Name
  - ✅ Должно быть сообщение об ошибке
- [ ] Заполните Name и попробуйте снова
  - ✅ Должно работать

---

### 8. ПАГИНАЦИЯ И ФИЛЬТРЫ

- [ ] Откройте Products
- [ ] Если >10 продуктов, должны быть упорядочены по страницам
- [ ] API использует PageSize=100 для админ-панели

---

### 9. ДАННЫЕ СОХРАНЕНИЯ

- [ ] Создайте новый продукт
- [ ] Закройте браузер полностью
- [ ] Откройте снова http://localhost:5001/login
- [ ] Залогиньтесь
- [ ] Откройте Products
- [ ] ✅ Ваш созданный продукт всё еще должен быть там (в БД)

---

### 10. SWAGGER ФУНКЦИИ

- [ ] Откройте http://localhost:5094/swagger
- [ ] Разверните каждый контроллер:
  - Auth ✅
  - Products ✅
  - Categories ✅
  - Brands ✅
  - Installments ✅
  - Users ✅
  - Orders ✅
  - Other...
- [ ] Попробуйте тестировать endpoints прямо в Swagger
- [ ] "Try it out" нажмите и заполните параметры
- [ ] Смотрите Response

---

## 🎉 ИТОГОВЫЙ РЕЗУЛЬТАТ

Если все чек-листы пройдены: ✅ **ПРОЕКТ ПОЛНОСТЬЮ РАБОТОСПОСОБЕН**

### Что работает:
✅ Web API с полной REST функциональностью
✅ JWT Authentication & Authorization  
✅ Admin Panel с красивым UI
✅ CRUD операции untuk всех сущностей
✅ Hover эффекты и styling
✅ Modal диалоги
✅ Responsive дизайн
✅ Seed данные
✅ Database persistence
✅ Form validation
✅ Navigation и routing

---

## 📝 ПРИМЕЧАНИЯ

- Если что-то не работает, проверьте консоль браузера (F12)
- Логи WebApi видны в консоли при запуске
- При ошибке 404 - проверьте что оба сервиса запущены
- Если tokен истек - пересоздайте через login

---

**Проект готов к использованию! 🚀**
