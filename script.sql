PGDMP     .                    {            ProjectManagement    15.1    15.1 3    R           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            S           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            T           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            U           1262    24618    ProjectManagement    DATABASE     �   CREATE DATABASE "ProjectManagement" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Russian_Russia.1251';
 #   DROP DATABASE "ProjectManagement";
                postgres    false            �            1259    24626 
   department    TABLE     a   CREATE TABLE public.department (
    id integer NOT NULL,
    name character varying NOT NULL
);
    DROP TABLE public.department;
       public         heap    postgres    false            �            1259    24651    employee    TABLE     �  CREATE TABLE public.employee (
    id integer NOT NULL,
    surname character varying,
    name character varying,
    patronymic character varying,
    "dateBirth" date,
    login character varying NOT NULL,
    password character varying NOT NULL,
    blocked boolean DEFAULT false NOT NULL,
    new boolean DEFAULT true NOT NULL,
    post integer,
    admin boolean DEFAULT false NOT NULL
);
    DROP TABLE public.employee;
       public         heap    postgres    false            �            1259    24756    message    TABLE     �   CREATE TABLE public.message (
    id integer NOT NULL,
    employee integer NOT NULL,
    task integer NOT NULL,
    text character varying NOT NULL
);
    DROP TABLE public.message;
       public         heap    postgres    false            �            1259    24743    notification    TABLE     �   CREATE TABLE public.notification (
    id integer NOT NULL,
    employee integer NOT NULL,
    text character varying NOT NULL,
    date date NOT NULL,
    new boolean DEFAULT true NOT NULL
);
     DROP TABLE public.notification;
       public         heap    postgres    false            �            1259    24693    phase    TABLE     z   CREATE TABLE public.phase (
    id integer NOT NULL,
    project integer NOT NULL,
    name character varying NOT NULL
);
    DROP TABLE public.phase;
       public         heap    postgres    false            �            1259    24633    post    TABLE     �   CREATE TABLE public.post (
    id integer NOT NULL,
    department integer NOT NULL,
    name character varying NOT NULL,
    "userGroup" boolean DEFAULT true NOT NULL
);
    DROP TABLE public.post;
       public         heap    postgres    false            �            1259    24670    project    TABLE     �   CREATE TABLE public.project (
    id integer NOT NULL,
    name character varying NOT NULL,
    "startDate" date NOT NULL,
    comment character varying,
    completed boolean DEFAULT false NOT NULL,
    "completDate" date
);
    DROP TABLE public.project;
       public         heap    postgres    false            �            1259    24678    projectEmployee    TABLE     g   CREATE TABLE public."projectEmployee" (
    employee integer NOT NULL,
    project integer NOT NULL
);
 %   DROP TABLE public."projectEmployee";
       public         heap    postgres    false            �            1259    24619    status    TABLE     ]   CREATE TABLE public.status (
    id integer NOT NULL,
    name character varying NOT NULL
);
    DROP TABLE public.status;
       public         heap    postgres    false            �            1259    24705    task    TABLE        CREATE TABLE public.task (
    id integer NOT NULL,
    phase integer NOT NULL,
    name character varying NOT NULL,
    executor integer NOT NULL,
    " dateComplet" date NOT NULL,
    status integer DEFAULT 1 NOT NULL,
    responsible integer NOT NULL,
    comment character varying
);
    DROP TABLE public.task;
       public         heap    postgres    false            �            1259    24728    taskLink    TABLE     j   CREATE TABLE public."taskLink" (
    "mainTask" integer NOT NULL,
    "dependentTask" integer NOT NULL
);
    DROP TABLE public."taskLink";
       public         heap    postgres    false            F          0    24626 
   department 
   TABLE DATA                 public          postgres    false    215   �:       H          0    24651    employee 
   TABLE DATA                 public          postgres    false    217   ;       O          0    24756    message 
   TABLE DATA                 public          postgres    false    224   p<       N          0    24743    notification 
   TABLE DATA                 public          postgres    false    223   +>       K          0    24693    phase 
   TABLE DATA                 public          postgres    false    220   �?       G          0    24633    post 
   TABLE DATA                 public          postgres    false    216   R@       I          0    24670    project 
   TABLE DATA                 public          postgres    false    218   
A       J          0    24678    projectEmployee 
   TABLE DATA                 public          postgres    false    219   �A       E          0    24619    status 
   TABLE DATA                 public          postgres    false    214   iB       L          0    24705    task 
   TABLE DATA                 public          postgres    false    221   �B       M          0    24728    taskLink 
   TABLE DATA                 public          postgres    false    222   �D       �           2606    24632    department department_pkey 
   CONSTRAINT     X   ALTER TABLE ONLY public.department
    ADD CONSTRAINT department_pkey PRIMARY KEY (id);
 D   ALTER TABLE ONLY public.department DROP CONSTRAINT department_pkey;
       public            postgres    false    215            �           2606    24659    employee employee_pkey 
   CONSTRAINT     T   ALTER TABLE ONLY public.employee
    ADD CONSTRAINT employee_pkey PRIMARY KEY (id);
 @   ALTER TABLE ONLY public.employee DROP CONSTRAINT employee_pkey;
       public            postgres    false    217            �           2606    24762    message message_pkey 
   CONSTRAINT     R   ALTER TABLE ONLY public.message
    ADD CONSTRAINT message_pkey PRIMARY KEY (id);
 >   ALTER TABLE ONLY public.message DROP CONSTRAINT message_pkey;
       public            postgres    false    224            �           2606    24750    notification notification_pkey 
   CONSTRAINT     \   ALTER TABLE ONLY public.notification
    ADD CONSTRAINT notification_pkey PRIMARY KEY (id);
 H   ALTER TABLE ONLY public.notification DROP CONSTRAINT notification_pkey;
       public            postgres    false    223            �           2606    24699    phase phase_pkey 
   CONSTRAINT     N   ALTER TABLE ONLY public.phase
    ADD CONSTRAINT phase_pkey PRIMARY KEY (id);
 :   ALTER TABLE ONLY public.phase DROP CONSTRAINT phase_pkey;
       public            postgres    false    220            �           2606    24640    post post_pkey 
   CONSTRAINT     L   ALTER TABLE ONLY public.post
    ADD CONSTRAINT post_pkey PRIMARY KEY (id);
 8   ALTER TABLE ONLY public.post DROP CONSTRAINT post_pkey;
       public            postgres    false    216            �           2606    24682 $   projectEmployee projectEmployee_pkey 
   CONSTRAINT     u   ALTER TABLE ONLY public."projectEmployee"
    ADD CONSTRAINT "projectEmployee_pkey" PRIMARY KEY (employee, project);
 R   ALTER TABLE ONLY public."projectEmployee" DROP CONSTRAINT "projectEmployee_pkey";
       public            postgres    false    219    219            �           2606    24677    project project_pkey 
   CONSTRAINT     R   ALTER TABLE ONLY public.project
    ADD CONSTRAINT project_pkey PRIMARY KEY (id);
 >   ALTER TABLE ONLY public.project DROP CONSTRAINT project_pkey;
       public            postgres    false    218            �           2606    24625    status status_pkey 
   CONSTRAINT     P   ALTER TABLE ONLY public.status
    ADD CONSTRAINT status_pkey PRIMARY KEY (id);
 <   ALTER TABLE ONLY public.status DROP CONSTRAINT status_pkey;
       public            postgres    false    214            �           2606    24732    taskLink taskLink_pkey 
   CONSTRAINT     q   ALTER TABLE ONLY public."taskLink"
    ADD CONSTRAINT "taskLink_pkey" PRIMARY KEY ("mainTask", "dependentTask");
 D   ALTER TABLE ONLY public."taskLink" DROP CONSTRAINT "taskLink_pkey";
       public            postgres    false    222    222            �           2606    24712    task task_pkey 
   CONSTRAINT     L   ALTER TABLE ONLY public.task
    ADD CONSTRAINT task_pkey PRIMARY KEY (id);
 8   ALTER TABLE ONLY public.task DROP CONSTRAINT task_pkey;
       public            postgres    false    221            �           2606    24779    employee employee_post_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.employee
    ADD CONSTRAINT employee_post_fkey FOREIGN KEY (post) REFERENCES public.post(id) NOT VALID;
 E   ALTER TABLE ONLY public.employee DROP CONSTRAINT employee_post_fkey;
       public          postgres    false    217    216    3224            �           2606    24641    post fk_post_of_department    FK CONSTRAINT     �   ALTER TABLE ONLY public.post
    ADD CONSTRAINT fk_post_of_department FOREIGN KEY (department) REFERENCES public.department(id);
 D   ALTER TABLE ONLY public.post DROP CONSTRAINT fk_post_of_department;
       public          postgres    false    215    216    3222            �           2606    24763    message message_employee_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.message
    ADD CONSTRAINT message_employee_fkey FOREIGN KEY (employee) REFERENCES public.employee(id);
 G   ALTER TABLE ONLY public.message DROP CONSTRAINT message_employee_fkey;
       public          postgres    false    224    3226    217            �           2606    24768    message message_task_fkey    FK CONSTRAINT     t   ALTER TABLE ONLY public.message
    ADD CONSTRAINT message_task_fkey FOREIGN KEY (task) REFERENCES public.task(id);
 C   ALTER TABLE ONLY public.message DROP CONSTRAINT message_task_fkey;
       public          postgres    false    224    3234    221            �           2606    24751 '   notification notification_employee_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.notification
    ADD CONSTRAINT notification_employee_fkey FOREIGN KEY (employee) REFERENCES public.employee(id);
 Q   ALTER TABLE ONLY public.notification DROP CONSTRAINT notification_employee_fkey;
       public          postgres    false    3226    223    217            �           2606    24700    phase phase_project_fkey    FK CONSTRAINT     y   ALTER TABLE ONLY public.phase
    ADD CONSTRAINT phase_project_fkey FOREIGN KEY (project) REFERENCES public.project(id);
 B   ALTER TABLE ONLY public.phase DROP CONSTRAINT phase_project_fkey;
       public          postgres    false    3228    220    218            �           2606    24683 -   projectEmployee projectEmployee_employee_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."projectEmployee"
    ADD CONSTRAINT "projectEmployee_employee_fkey" FOREIGN KEY (employee) REFERENCES public.employee(id);
 [   ALTER TABLE ONLY public."projectEmployee" DROP CONSTRAINT "projectEmployee_employee_fkey";
       public          postgres    false    3226    219    217            �           2606    24688 ,   projectEmployee projectEmployee_project_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."projectEmployee"
    ADD CONSTRAINT "projectEmployee_project_fkey" FOREIGN KEY (project) REFERENCES public.project(id);
 Z   ALTER TABLE ONLY public."projectEmployee" DROP CONSTRAINT "projectEmployee_project_fkey";
       public          postgres    false    219    218    3228            �           2606    24784 $   taskLink taskLink_dependentTask_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."taskLink"
    ADD CONSTRAINT "taskLink_dependentTask_fkey" FOREIGN KEY ("dependentTask") REFERENCES public.task(id) NOT VALID;
 R   ALTER TABLE ONLY public."taskLink" DROP CONSTRAINT "taskLink_dependentTask_fkey";
       public          postgres    false    3234    222    221            �           2606    24733    taskLink taskLink_mainTask_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."taskLink"
    ADD CONSTRAINT "taskLink_mainTask_fkey" FOREIGN KEY ("mainTask") REFERENCES public.task(id);
 M   ALTER TABLE ONLY public."taskLink" DROP CONSTRAINT "taskLink_mainTask_fkey";
       public          postgres    false    222    3234    221            �           2606    24718    task task_executor_fkey    FK CONSTRAINT     z   ALTER TABLE ONLY public.task
    ADD CONSTRAINT task_executor_fkey FOREIGN KEY (executor) REFERENCES public.employee(id);
 A   ALTER TABLE ONLY public.task DROP CONSTRAINT task_executor_fkey;
       public          postgres    false    3226    221    217            �           2606    24713    task task_phase_fkey    FK CONSTRAINT     q   ALTER TABLE ONLY public.task
    ADD CONSTRAINT task_phase_fkey FOREIGN KEY (phase) REFERENCES public.phase(id);
 >   ALTER TABLE ONLY public.task DROP CONSTRAINT task_phase_fkey;
       public          postgres    false    220    221    3232            �           2606    24723    task task_responsible_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.task
    ADD CONSTRAINT task_responsible_fkey FOREIGN KEY (responsible) REFERENCES public.employee(id);
 D   ALTER TABLE ONLY public.task DROP CONSTRAINT task_responsible_fkey;
       public          postgres    false    221    217    3226            �           2606    24773    task task_status_fkey    FK CONSTRAINT     ~   ALTER TABLE ONLY public.task
    ADD CONSTRAINT task_status_fkey FOREIGN KEY (status) REFERENCES public.status(id) NOT VALID;
 ?   ALTER TABLE ONLY public.task DROP CONSTRAINT task_status_fkey;
       public          postgres    false    3220    221    214            F   y   x���v
Q���W((M��L�KI-H,*�M�+Q��L�Q�K�M�Ts�	uV�0�QP�0�bㅭ
@b�Ŧ��/l����.�P״��$�L#����캰��& �r��b�2�� �D�      H   N  x�͓ON�@��=Ť]�&`�A#ƕ�.Hۺ�0Z*��@v�t��C�&Mc=�p#g����o�{��͘��@3�� L�c�`/t�c0tl�$�M����G��z�%��m��ډ�E_n���|O�*��ŹXϘ	�X�X��9�<\��OE �O�ӌ��ͅ�gژ�:K�,˒�$$�}�6w��D�`I 3��Kps�Z�.{�!��4+)?��պ��3ƬH�cs���q"����N^��n�]r�ԋG%���U-9]/tK7M=���x-�����L�Xe[��w�8Ⱥ�.!8j��YbXFBK��=�j��S�PSd��E�s      O   �  x��S_K�P�S��T����C� !�AZ$-A�z�փ	�A`�7��l�9�¹ߨsO�����n�gp~��K��Y���)��k�ƥ��;���F]��n�=��j熞�C7G'�2�42
dH�0��1:��J��H�3�xg�1��8\P1�	�]���� ~�e���}�Ж�^o$�?�lzt��fC��x�)�1 ����Jm�������.]��Abs�0�A|_�1����bF����%����U����z��ft��)�jt�젶��}�¾���}���	�X��Og�n�ȯfq|]�wo�w�g%��$s�#?� k�kd̀��-�A���
��$=3�r�
3�e\|�L%+�3i-&�����,��k����h3�W�m�2�
h9��-/a�_�>\      N   �  x���JQ��>���	�8S��"Z�� �����α����&�W��ɡtz�sߨ�ަ�"�E��v��s����3��,�o)�Yܢ��^�*�ku�ڷ�%۪�hΪhd4��S���6Ol�*%�y��ݵ�����%�Jt����O����G~����e��!�hr��G��އ���c>�ɦ2ˈ��#3����R�K��s�]��R���骜�HW*�i��9�4�M��m9.�D�x�� �6��|����I��&Y����:V�B�`��f�Uhؖ�C��bx@�U�g/J���bط[�<K�
�p��w�8���>>G6P�UB�������7���X�!x@5�$;�^��<P�}e���jI��'ό��,Nȿg�{�4ٜI�. ���՟�����&���b/���      K   d   x���v
Q���W((M��L�+�H,NU��L�Q((��JM.�Q�K�M�Ts�	uV�0�Q0�QP����/lP0T״��$�$�1F�1��$#�I\\ ��J:      G   �   x���v
Q���W((M��L�+�/.Q��L�QHI-H,*�M�+�Q�K�M�QP*-N-r/�/-P�Ts�	uV�0�Q "�/��t��b�-�^�qa���BIQi��5�'�1�Q0Y3��V��[/6\�pa�Ş{/v_ة �sPh�]���h ڝ��S��� ��\�      I   �   x���v
Q���W((M��L�+(��JM.Q��L�Q�K�M�QP*.I,*qI,IU�QH���M�+3
rRKR�ʔ�l�M�0G�P�`c��/6\�wa�]�.쿰O�¦{��-@�@֎�}
�A�
��ƋM@�=�ՁZ���uLu�L�<�P��Ĝ�TGӚ˓��7�~?���e`
wTIQi*\���i�6#t����SG... ����      J   c   x���v
Q���W((M��L�S*(��JM.q�-�ɯLMUR�H�2u�r�
a�>���
�:
���\�T0ǈ�Q��{��a�1��1��9F���� �͠=      E   n   x���v
Q���W((M��L�+.I,)-V��L�Q�K�M�Ts�	uV�0�QP�0�¾�.l�د�i��I�v#��I
.l���¾�M��l�1Ȍ�`� g��� G�HE      L      x�Ŕ�j�@��~�C6J@6�ȷ�U)Y�Mҽ�j��]&n�,r�&�U)�8�j�-;�p��K�:����̹ͧ��R�qx��j���46;�V���ϴ�>�i��v�zvO��z�S�v����w���x;:���]�Nw���f�~����=���~|pH�B'S'�x�3��+���TP��弱��m��{or���S��Z̲��l��>+��)p|f
���Fv���*7�W076\��a9E�҆� ��4��+~�KG�8Dߜ}9"~�g�����9.��@d�+�Gy��f�>�u���*$oy���� ��W��UI��U�y�#!;0.G4�@۪r�A~ h�:�KM ��[Fv��������v�c��>RO^9Hڲ��-��ᶿx�76�}^ț���F��~G	\&/���B��J��y�C��[�yu!�J�b�H�Ȓhy��x�<>� ������kS�1����i�|����m�g�\�����      M   o   x���v
Q���W((M��L�S*I,�����VR�P�M���t�RRR�RR�J��
a�>���
�:
&��\�T0�DG��JF��(�S�(s*e��`h 4�� e>qx     