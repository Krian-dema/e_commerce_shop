<script setup>
import { ref } from 'vue';
import SlIconArrow from '@/components/common/icons/SlIconArrow.vue';

defineProps({
  item: {
    type: Object,
    required: true,
  },
});

const showPanel = ref(false);

const togglePanel = () => {
  showPanel.value = !showPanel.value;
};
</script>

<template>
  <div class="border rounded-2xl py-4 lg:mx-4" :class="{ active: showPanel }">
    <div class="font-bold cursor-pointer px-8" @click="togglePanel">
      <div class="hidden md:flex justify-between items-center w-full h-full">
        <div class="flex flex-row items-center gap-6">
          <slot name="headerImage" :item="item"></slot>
          <slot name="headerContent" :item="item"></slot>
        </div>

        <div class="flex flex-row items-center gap-6">
          <slot name="counter" :item="item"></slot>
          <span class="transition" :class="{ 'rotate-90': showPanel }">
            <SlIconArrow class="h-5 w-5 text-primary-500 stroke-4" />
          </span>
        </div>
      </div>

      <div class="flex md:hidden flex-col gap-4 w-full">
        <div class="flex flex-row justify-between">
          <div class="w-3/5 h-3/5">
            <slot name="headerImage" :item="item"></slot>
          </div>
          <div class="flex flex-row items-center gap-4">
            <slot name="counter" :item="item"></slot>
            <span class="transition" :class="{ 'rotate-90': showPanel }">
              <SlIconArrow class="h-5 w-5 text-primary-500 stroke-4" />
            </span>
          </div>
        </div>

        <slot name="headerContent" :item="item"></slot>
      </div>
    </div>
    <Transition name="list">
      <div v-show="showPanel" class="px-4 mt-4">
        <slot name="content" :item="item"></slot>
      </div>
    </Transition>
  </div>
</template>

<style scoped>
.active {
  @apply bg-light-gray-20% mx-0;
}

.list-enter-active,
.list-leave-active {
  transition: opacity 0.15s ease;
}

.list-enter-from,
.list-leave-to {
  opacity: 0;
}
</style>
