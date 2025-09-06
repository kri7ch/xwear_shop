<template>
    <nav>
        <div class="nameShop">
            <h1 class="shopName">XWEAR</h1>
        </div>

        <div class="menu">
            <ul>
                <li><a href="#">Одежда</a>
                    <ul>
                        <li><a href="#">Женская одежда</a></li>
                        <li><a href="#">Мужская одежда</a></li>
                        <li><a href="#">Детская одежда</a></li>
                    </ul>
                </li>

                <li><a href="#">Обувь</a>
                    <ul>
                        <li><a href="#">Женская обувь</a></li>
                        <li><a href="#">Мужская обувь</a></li>
                        <li><a href="#">Спортивная обувь</a></li>
                    </ul>
                </li>

                <li><a href="#">Аксессуары</a>
                    <ul>
                        <li><a href="#">Сумки и рюкзаки</a></li>
                        <li><a href="#">Головные уборы</a></li>
                        <li><a href="#">Бижутерия</a></li>
                    </ul>
                </li>

                <li>
                    <a class="menu-caret" href="#">Бренды</a>
                    <ul>
                        <li><a href="#">Nike</a></li>
                        <li><a href="#">Adidas</a></li>
                        <li><a href="#">Zara</a></li>
                    </ul>
                </li>

                <li><a href="#">Расчет стоимости</a>
                    <ul>
                        <li><a href="#">Калькулятор доставки</a></li>
                        <li><a href="#">Таблица размеров</a></li>
                        <li><a href="#">Стоимость услуг</a></li>
                    </ul>
                </li>

                <li><a href="#">Информация</a>
                    <ul>
                        <li><a href="#">О нас</a></li>
                        <li><a href="#">Доставка и оплата</a></li>
                        <li><a href="#">Контакты</a></li>
                    </ul>
                </li>
            </ul>
        </div>

        <div class="link-icon">
            <a href="#">
                <img src="../assets/images/icons/search_Icon.svg" alt="">
            </a>

            <a href="#">
                <img src="../assets/images/icons/star_icon.svg" alt="">
            </a>

            <a href="#">
                <img src="../assets/images/icons/user_icon.svg" alt="">
            </a>

            <a href="#" class="bascket-info-link">
                <img src="../assets/images/icons/bascket_icon.svg" alt="">
            </a>
        </div>
    </nav>
</template>

<script>
export default {
  name: 'AppNavbar'
}
</script>
