# Генерируем тестовые файлы сертификатов в PS

Открываем PowerShell под админом.
Подробнее о создании [здесь](https://learn.microsoft.com/en-us/powershell/module/pki/new-selfsignedcertificate?view=windowsserver2022-ps)

```powershell
# Создаем test cert Serial #1 (Все поля ОК)
$params = @{
    SerialNumber=00000001
    Subject = 'CN=Тестиков Тест Тестович,O=Тестовое управление,OU=Тестовый отдел №1,SN=Тестиков,G=Тест Тестович,T=Тестер,1.2.643.100.3=12345678901,1.2.643.3.131.1.1=123456789012,E=testikoff@testmail.com'
    TextExtension = @(
        '1.2.643.100.111={text}TestPro Tool',
        '1.2.643.100.112={text}Test Pro CA Tool' )
    KeyUsage = 'DigitalSignature'
    KeyAlgorithm = 'RSA'
    KeyLength = 2048
    CertStoreLocation = 'Cert:\CurrentUser\my'
}

New-SelfSignedCertificate @params

# Экспорт из хранилища 'Текущий пользователь|Личное' в файл
$params = @{
    SerialNumber = '00000001'
}
Set-Location -Path Cert:\LocalMachine\My
$cert = Get-Certificate @params
Export-Certificate -Cert $cert -FilePath C:\00000001.cer
```

