using System;
using System.Collections.Generic;

namespace Libraryhub.Contracts.V1
{
    public class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

     

        public static class Identity
        {
            public const string Login = Base + "/identity/login";
            public const string Register = Base + "/identity/register";
            public const string RefreshToken = Base + "/identity/refresh";
            public const string UserProfile = Base + "/identity/profile";
        }

        public static class Book
        {
            public const string LOAD_ALL_BOOKS_ENDPOINT = Base + "/book/LoadAllBooks";
            public const string LOAD_SINGLE_BOOK_BY_ID_ENDPOINT = Base + "/book/LoadSingleBook/{bookId}";
            public const string UPDATE_BOOK_ENDPOINT = Base + "/book/UpdateBook/{bookId}";
            public const string DELETE_BOOK_ENDPOINT = Base + "/book/DeleteBook/{bookId}";
            public const string CREATE_BOOK_ENDPOINT = Base + "/book/CreateBook";
            public const string BOOK_SEARCH_ENDPOINT = Base + "/book/Search";

            public const string CHECK_OUT_BOOKS_ENDPOINT = Base + "/book/CheckOutBooks";
            public const string CHECK_IN_BOOKS_ENDPOINT = Base + "/book/CheckInBooks";

            public const string GET_ALL_BOOK_PEANALTY_CHARGIES_ENDPOINT = Base + "/book/GetAllPenaltyChargies";
            public const string GET_CUSTOMER_PENALTY_CHARGIES_ENDPOINT = Base + "/book/GetCustomerPenaltyChargies";
        }

        public static class Settings
        {
            public const string CHECKOUT_REMINDER_ENDPOINT = Base + "/reminder/Switch";
        }

        public static class Order
        {
            public const string ORDER_ENDPOINT = Base + "/order/CreateOrder";
        }

    }
}
