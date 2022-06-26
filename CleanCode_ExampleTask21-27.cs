using System;

namespace CleanCodeTask9
{
    class CleanCode_ExampleTask21_27
    {
        private const string Message = "Введите серию и номер паспорта";
        private const int MinPassportLength = 10;
        private const string PassportQuery = "select * from passports where num='{0}' limit 1;";
        private const string ConnectionPath = "Data Source=" +
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\db.sqlite";
        private const string ErrorMessage = "Файл db.sqlite не найден. Положите файл в папку вместе с exe.";

        public enum ResultCode
        {
            AccessGranted,
            AccessDenied,
            NoUserRegistered,
            NotValidPassportFormat,
            UnexpectedError
        };

        private void CheckButton_Click(object sender, EventArgs e)
        {
            ResultCode resultCode = ResultCode.UnexpectedError;

            string userPassportNumber = this.passportTextBox.Text.Trim().Replace(" ", string.Empty);

            if (userPassportNumber == string.Empty)
            {
                MessageBox.Show(Message);
                return;
            }

            if (userPassportNumber.Length >= MinPassportLength)
                resultCode = CheckUserAccessToVoting(userPassportNumber);
            else
                resultCode = ResultCode.NotValidPassportFormat;

            this.textResult.Text = GetResultMessage(resultCode, userPassportNumber);
        }

        private ResultCode CheckUserAccessToVoting(string userPassportNumber)
        {
            string commandText = string.Format(PassportQuery,
                (object)Form.ComputeSha256Hash(userPassportNumber));

            DataTable dataTable = TryGetDataTable(commandText);

            if (dataTable.Rows.Count == 0)
                return ResultCode.NoUserRegistered;

            if (Convert.ToBoolean(dataTable.Rows[0].ItemArray[1]))
                return ResultCode.AccessGranted;
            else
                return ResultCode.AccessDenied;
        }

        private void TryGetDataTable(string commandText)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string connectionString = string.Format(ConnectionPath);

                SQliteConnection connection = new SQLiteConnection(connectionString);

                connection.Open();

                SQLiteDataAdapter sqLiteDataAdapter = new SQLiteDataAdapter(
                    new SQLiteCommand(commandText, connection));

                sqLiteDataAdapter.Fill(dataTable);

                connection.Close();
            }
            catch (SQLiteException exception)
            {
                if (exception.ErrorCode == 1)
                    MessageBox.Show(ErrorMessage);
            }

            return dataTable;
        }

        private string GetResultMessage(ResultCode resultCode, string passportNumber)
        {
            return resultCode switch
            {
                ResultCode.AccessGranted => "По паспорту «" + passportNumber +
                "» доступ к бюллетеню на дистанционном электронном голосовании ПРЕДОСТАВЛЕН",
                ResultCode.AccessDenied => "По паспорту «" + passportNumber +
                "» доступ к бюллетеню на дистанционном электронном голосовании НЕ ПРЕДОСТАВЛЯЛСЯ",
                ResultCode.NoUserRegistered => "Паспорт «" + passportNumber +
                "» в списке участников дистанционного голосования НЕ НАЙДЕН",
                ResultCode.NotValidPassportFormat => "Неверный формат серии или номера паспорта",
                _ => "Непредвиденная ошибка",
            };
        }
    }
}
