# Sample application based on django tutorial
https://docs.djangoproject.com/en/3.2/intro/

## Use django-pyodbc from github repository
* pip3 install git+https://github.com/cpyang/django-pyodbc.git  
* Modify mysite/settings.py with Tibero as default database  
  ```yaml
    DATABASES = {  
        'default': {  
            'ENGINE': "django_pyodbc",  
            'HOST': "127.0.0.1,8629",  
            'USER': "tibero",  
            'PASSWORD': "tmax",  
            'NAME': "tibero",  
            'OPTIONS': {  
                'host_is_server': True,  
                'driver': "Tibero",  
                'dbms_type': 'tibero',  
            },  
        }  
    }  
  ```
* Setup Environment Variables
    . ./setenv.sh
* Initialise django database
    python3 manage.py migrate
* After made change to models
    python3 manage.py makemigrations polls  
    python3 manage.py sqlmigrate polls 0001  
* Create site admin user
    python3 manage.py createsuperuser
* Start django web service
    python3 manage.py runserver
* Access web application
    http://localhost:8000/admin  
    http://localhost:8000/polls  
