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
            public const string LOAD_ALL_BOOKS_ENDPOINT = Base + "/book/loadAllBooks";
            public const string LOAD_SINGLE_BOOK_BY_ID_ENDPOINT = Base + "/book/loadSingleBook/{bookId}";
            public const string UPDATE_BOOK_ENDPOINT = Base + "/book/pdateBook/{bookId}";
            public const string DELETE_BOOK_ENDPOINT = Base + "/book/deleteBook/{bookId}";
            public const string CREATE_BOOK_ENDPOINT = Base + "/book/createBook";
            public const string BOOK_SEARCH_ENDPOINT = Base + "/book/search";

            public const string CHECK_OUT_BOOKS_ENDPOINT = Base + "/book/checkOutBooks";
            public const string CHECK_IN_BOOKS_ENDPOINT = Base + "/book/checkInBooks";

            public const string GET_ALL_BOOK_PEANALTY_CHARGIES_ENDPOINT = Base + "/book/getAllPenaltyChargies";
            public const string GET_CUSTOMER_PENALTY_CHARGIES_ENDPOINT = Base + "/book/getCustomerPenaltyChargies";
        }

        public static class Settings
        {
            public const string CHECKOUT_REMINDER_ENDPOINT = Base + "/reminder/switch/customerReminder";
            public const string LEFT_OVER_REMINDER_ENDPOINT = Base + "/reminder/switch/leftOverReminder";
        }

        public static class Order
        {
            public const string ORDER_ENDPOINT = Base + "/order/createOrder";
            public const string CONFIRM_ORDER_ENDPOINT = Base + "/order/confirmOrder";
            public const string SEARCH_ORDERS_ENDPOINT = Base + "/order/search";
            public const string ORDER_DETAILS_ENDPOINT = Base + "/order/details"; 
            public const string SEARCH_ORDER_DETAILS_ENDPOINT = Base + "/order/DetailsSearch";
        } 

    }
}
