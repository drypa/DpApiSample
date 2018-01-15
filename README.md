# DpApi usage sample

This is Proof Of Concept code to replace ProtectSection from *System.Configuration.ConfigurationSection* .

The problem with ProtectSection of *System.Configuration* is strong coupling to concrete assembly. For example to me
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <configSections>
        <section name="credentials" type="MyApplication.SensetiveDataStorage, MyApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" allowLocation="true" allowDefinition="Everywhere" allowExeDefinition="MachineToApplication" overrideModeDefault="Allow" restartOnExternalChanges="true" requirePermission="true" />
    </configSections>
    <credentials configProtectionProvider="DataProtectionConfigurationProvider">
        <EncryptedData>
            <CipherData>
                <CipherValue>...</CipherValue>
            </CipherData>
        </EncryptedData>
    </credentials>
</configuration>
```
And if any part of the specified part of type (*MyApplication.SensetiveDataStorage, MyApplication, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = null*) will changed, I can't read this data anymore:

> System.Configuration.ConfigurationErrorsException: An error occurred creating the configuration section handler for credentials: Could > not load file or assembly 'MyApplication.SensetiveDataStorage, MyApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies. The located assembly's manifest definition does not match the assembly reference.

but there are some workaround, to not write full type definition(include full assembly definition) but only for example full type name and assembly name:

```csharp
Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
config.TypeStringTransformer = fullTypeName => string.Join(",", fullTypeName.Split(',').Take(2));
```
but I think that is so dirty
