<template>
    <nav>
        <div class="nameShop">
            <h1 class="shopName">XWEAR</h1>
        </div>

        <div class="menu">
            <ul>
                <li><a href="#">Одежда</a>
                    <ul>
                        <li><a href="#">1</a></li>
                        <li><a href="#">2</a></li>
                        <li><a href="#">3</a></li>
                    </ul>
                </li>

                <li><a href="#">Обувь</a>
                    <ul>
                        <li><a href="#">1</a></li>
                        <li><a href="#">2</a></li>
                        <li><a href="#">3</a></li>
                    </ul>
                </li>

                <li><a href="#">Аксессуары</a>
                    <ul>
                        <li><a href="#">1</a></li>
                        <li><a href="#">2</a></li>
                        <li><a href="#">3</a></li>
                    </ul>
                </li>

                <li>
                    <a class="menu-caret" href="#">Бренды</a>
                    <ul>
                        <li><a href="#">1</a></li>
                        <li><a href="#">2</a></li>
                        <li><a href="#">3</a></li>
                    </ul>
                </li>

                <li><a href="#">Расчет стоимости</a>
                    <ul>
                        <li><a href="#">1</a></li>
                        <li><a href="#">2</a></li>
                        <li><a href="#">3</a></li>
                    </ul>
                </li>

                <li><a href="#">Информация</a>
                    <ul>
                        <li><a href="#">1</a></li>
                        <li><a href="#">2</a></li>
                        <li><a href="#">3</a></li>
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
